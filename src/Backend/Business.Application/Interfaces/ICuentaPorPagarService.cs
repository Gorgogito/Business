namespace Business.Application.Interfaces;

using Business.Application.DTOs.Finanzas;
using Business.Domain.Entities.Compras;

public interface ICuentaPorPagarService
{
    Task<IEnumerable<CuentaPorPagarDto>> GetAllAsync();
    Task<IEnumerable<CuentaPorPagarDto>> GetPendientesAsync();
    Task<IEnumerable<CuentaPorPagarDto>> GetByProveedorAsync(int proveedorId);
    Task<CuentaPorPagarDto?> GetByIdAsync(int id);

    /// <summary>Crea la cuenta por pagar de una recepción. Agrega al contexto sin guardar (atómico con la recepción).</summary>
    Task GenerarDesdeRecepcionAsync(RecepcionCompra recepcion, int proveedorId, decimal montoTotal, int diasCredito, string? userName, CancellationToken ct = default);

    /// <summary>Registra un pago que reduce el saldo. Persiste el cambio.</summary>
    Task<PagoDto?> RegistrarPagoAsync(int cuentaPorPagarId, CreatePagoDto dto, string userName, CancellationToken ct = default);
}
