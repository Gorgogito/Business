namespace Business.Application.Interfaces;

using Business.Application.DTOs.Contabilidad;

public interface IAsientoContableService
{
    Task<IEnumerable<AsientoContableDto>> GetAllAsync();
    Task<AsientoContableDto?> GetByIdAsync(int id);
    Task<IEnumerable<AsientoContableDto>> GetByPeriodoAsync(DateTime desde, DateTime hasta);
    Task<AsientoContableDto> CreateAsync(CreateAsientoContableDto dto, string userName);
    Task<AsientoContableDto?> AnularAsync(int id, string userName);

    /// <summary>
    /// Genera un asiento automático a partir de líneas identificadas por código de cuenta.
    /// Ignora líneas en cero; si quedan menos de dos, no registra nada. No llama a
    /// SaveChanges: se persiste junto con el documento que lo origina.
    /// </summary>
    Task GenerarAsientoAsync(
        DateTime fecha, string glosa, string tipo, string? referencia,
        IEnumerable<(string CuentaCodigo, decimal Debe, decimal Haber)> lineas,
        string? userName, CancellationToken ct = default);
}
