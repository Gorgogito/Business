namespace Business.Persistence.Auditing;

using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Business.Domain.Entities.Auditoria;

/// <summary>
/// Acumula la información de auditoría de una entidad durante SaveChanges. Las altas
/// generan el Id recién en la base, por eso las claves temporales se resuelven después
/// de guardar (ver <see cref="HasTemporaryProperties"/>).
/// </summary>
internal class AuditEntry
{
    // Propiedades que nunca se registran (sensibles o ruidosas).
    private static readonly HashSet<string> Excluded = new(StringComparer.OrdinalIgnoreCase)
    {
        "PasswordHash", "RefreshToken", "RefreshTokenExpiry"
    };

    public EntityEntry Entry { get; }
    public string TableName { get; }
    public string Action { get; }
    public string? UserName { get; }
    public Dictionary<string, object?> OldValues { get; } = new();
    public Dictionary<string, object?> NewValues { get; } = new();
    public Dictionary<string, object?> KeyValues { get; } = new();
    public List<PropertyEntry> TemporaryProperties { get; } = new();

    public bool HasTemporaryProperties => TemporaryProperties.Count > 0;

    /// <summary>
    /// Indica si vale la pena registrar la entrada. Una modificación en la que solo
    /// cambiaron propiedades excluidas (p. ej. el RefreshToken al iniciar sesión) no
    /// aporta información y se descarta para no ensuciar la bitácora.
    /// </summary>
    public bool IsMeaningful =>
        Entry.State != EntityState.Modified || OldValues.Count > 0 || NewValues.Count > 0;

    public AuditEntry(EntityEntry entry, string? userName)
    {
        Entry = entry;
        TableName = entry.Metadata.GetTableName() ?? entry.Entity.GetType().Name;
        Action = entry.State.ToString();
        UserName = userName;

        foreach (var property in entry.Properties)
        {
            var name = property.Metadata.Name;

            if (property.IsTemporary)
            {
                // El valor definitivo (Id) se conocerá después de guardar.
                TemporaryProperties.Add(property);
                continue;
            }

            if (property.Metadata.IsPrimaryKey())
            {
                KeyValues[name] = property.CurrentValue;
                continue;
            }

            if (Excluded.Contains(name)) continue;

            switch (entry.State)
            {
                case EntityState.Added:
                    NewValues[name] = property.CurrentValue;
                    break;
                case EntityState.Deleted:
                    OldValues[name] = property.OriginalValue;
                    break;
                case EntityState.Modified when property.IsModified:
                    OldValues[name] = property.OriginalValue;
                    NewValues[name] = property.CurrentValue;
                    break;
            }
        }
    }

    public AuditLog ToAuditLog()
    {
        // Resuelve las claves/valores que eran temporales al momento de la captura.
        foreach (var prop in TemporaryProperties)
        {
            if (prop.Metadata.IsPrimaryKey())
                KeyValues[prop.Metadata.Name] = prop.CurrentValue;
            else
                NewValues[prop.Metadata.Name] = prop.CurrentValue;
        }

        return new AuditLog
        {
            TableName = TableName,
            Action = Action,
            EntityId = string.Join(",", KeyValues.Values),
            OldValues = OldValues.Count == 0 ? null : JsonSerializer.Serialize(OldValues),
            NewValues = NewValues.Count == 0 ? null : JsonSerializer.Serialize(NewValues),
            UserName = UserName,
            Timestamp = DateTime.UtcNow
        };
    }
}
