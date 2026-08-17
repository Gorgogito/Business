namespace Business.Persistence.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.Interfaces;
using Business.Domain.Entities.Configuration;
using Business.Persistence.Context;

/// <summary>
/// Implementación de correlativos que incrementa el contador con una única sentencia
/// UPDATE ... OUTPUT. Al ser atómica a nivel de fila en SQL Server, dos peticiones
/// concurrentes nunca obtienen el mismo número (a diferencia de Count()+1 o Random()).
/// La numeración es independiente por empresa (multiempresa); si una empresa aún no tiene
/// su contador para un tipo/serie, se crea automáticamente a partir de la empresa base.
/// </summary>
public class CorrelativoService : ICorrelativoService
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService? _currentUser;

    public CorrelativoService(ApplicationDbContext context, ICurrentUserService? currentUser = null)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<string> SiguienteAsync(string tipoDocumento, string serie, int empresaId = 1, CancellationToken ct = default)
    {
        // La empresa del usuario autenticado tiene prioridad; el parámetro es el fallback.
        var empresa = _currentUser?.EmpresaId ?? empresaId;

        var next = await IncrementarAsync(tipoDocumento, serie, empresa, ct);
        if (next == null)
        {
            // La empresa aún no tiene contador para este tipo/serie: se crea y se reintenta.
            await AutoProvisionarAsync(tipoDocumento, serie, empresa, ct);
            next = await IncrementarAsync(tipoDocumento, serie, empresa, ct);
        }

        if (next == null)
            throw new InvalidOperationException(
                $"No se pudo obtener el correlativo para TipoDocumento='{tipoDocumento}', Serie='{serie}', EmpresaId={empresa}.");

        return $"{next.Prefijo}{next.UltimoNumero.ToString().PadLeft(next.Longitud, '0')}";
    }

    private async Task<CorrelativoNext?> IncrementarAsync(string tipoDocumento, string serie, int empresa, CancellationToken ct)
    {
        var filas = await _context.Database
            .SqlQueryRaw<CorrelativoNext>(
                @"UPDATE Correlativos
                     SET UltimoNumero = UltimoNumero + 1,
                         UpdatedAt = SYSUTCDATETIME()
                   OUTPUT INSERTED.UltimoNumero AS UltimoNumero,
                          INSERTED.Longitud     AS Longitud,
                          INSERTED.Prefijo      AS Prefijo
                   WHERE TipoDocumento = {0} AND Serie = {1} AND EmpresaId = {2}",
                tipoDocumento, serie, empresa)
            .ToListAsync(ct);
        return filas.Count > 0 ? filas[0] : null;
    }

    /// <summary>
    /// Crea el contador de la empresa copiando la configuración (longitud/prefijo) de la
    /// empresa base. Usa INSERT crudo para no interferir con el ChangeTracker del contexto.
    /// </summary>
    private async Task AutoProvisionarAsync(string tipoDocumento, string serie, int empresa, CancellationToken ct)
    {
        var plantilla = await _context.Set<Correlativo>().AsNoTracking()
            .FirstOrDefaultAsync(c => c.TipoDocumento == tipoDocumento && c.Serie == serie && c.EmpresaId == 1, ct);
        var longitud = plantilla?.Longitud ?? 8;
        var prefijo = plantilla?.Prefijo ?? "";

        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"INSERT INTO Correlativos (TipoDocumento, Serie, EmpresaId, UltimoNumero, Longitud, Prefijo, IsActive, CreatedAt)
                  VALUES ({0}, {1}, {2}, 0, {3}, {4}, 1, SYSUTCDATETIME())",
                new object[] { tipoDocumento, serie, empresa, longitud, prefijo }, ct);
        }
        catch (DbUpdateException) { /* otra petición lo creó primero: se reintenta el UPDATE */ }
        catch (Microsoft.Data.SqlClient.SqlException) { /* violación de índice único por carrera */ }
    }

    /// <summary>Proyección del resultado de la sentencia OUTPUT.</summary>
    private sealed class CorrelativoNext
    {
        public int UltimoNumero { get; set; }
        public int Longitud { get; set; }
        public string Prefijo { get; set; } = string.Empty;
    }
}
