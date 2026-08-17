namespace Business.Domain.Entities.Finanzas;

using Business.Domain.Common;

/// <summary>Pago recibido de un cliente, aplicado a una cuenta por cobrar.</summary>
public class Cobro : BaseEntity
{
    public int CuentaPorCobrarId { get; set; }
    public CuentaPorCobrar? CuentaPorCobrar { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public decimal Monto { get; set; }
    public string MedioPago { get; set; } = "EFECTIVO"; // EFECTIVO, TRANSFERENCIA, TARJETA, CHEQUE
    public string? Referencia { get; set; }
}
