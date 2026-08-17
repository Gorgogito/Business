namespace Business.Domain.Entities.Auditoria;

/// <summary>
/// Registro de auditoría: una fila por cada alta, modificación o baja de una entidad
/// de negocio, con los valores antes/después y el usuario responsable. No hereda de
/// BaseEntity a propósito, para no auditarse a sí misma.
/// </summary>
public class AuditLog
{
    public int Id { get; set; }
    public string TableName { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty; // Added, Modified, Deleted
    public string? OldValues { get; set; }              // JSON (null en altas)
    public string? NewValues { get; set; }              // JSON (null en bajas)
    public string? UserName { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
