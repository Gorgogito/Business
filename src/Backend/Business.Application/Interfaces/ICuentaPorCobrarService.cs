namespace Business.Application.Interfaces;

using Business.Application.DTOs.Finanzas;
using Business.Domain.Entities.Ventas;

public interface ICuentaPorCobrarService
{
    Task<IEnumerable<CuentaPorCobrarDto>> GetAllAsync();
    Task<IEnumerable<CuentaPorCobrarDto>> GetPendientesAsync();
    Task<IEnumerable<CuentaPorCobrarDto>> GetByClienteAsync(int clienteId);
    Task<CuentaPorCobrarDto?> GetByIdAsync(int id);

    /// <summary>Crea la cuenta por cobrar de una factura. Agrega al contexto sin guardar (atómico con la factura).</summary>
    Task GenerarDesdeFacturaAsync(Factura factura, int diasCredito, string? userName, CancellationToken ct = default);

    /// <summary>Anula la cuenta por cobrar asociada a una factura. No guarda. Falla si ya tuvo cobros.</summary>
    Task AnularPorFacturaAsync(int facturaId, CancellationToken ct = default);

    /// <summary>Registra un cobro que reduce el saldo. Persiste el cambio.</summary>
    Task<CobroDto?> RegistrarCobroAsync(int cuentaPorCobrarId, CreateCobroDto dto, string userName, CancellationToken ct = default);

    /// <summary>Aplica una nota de crédito: reduce el saldo pendiente de la factura. No guarda.</summary>
    Task AplicarNotaCreditoAsync(int facturaId, decimal monto, CancellationToken ct = default);

    /// <summary>Aplica una nota de débito: incrementa el saldo y el monto de la factura. No guarda.</summary>
    Task AplicarNotaDebitoAsync(int facturaId, decimal monto, CancellationToken ct = default);
}
