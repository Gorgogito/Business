namespace Business.Domain.Entities.Finanzas;

using Business.Domain.Common;

/// <summary>Pago efectuado a un proveedor, aplicado a una cuenta por pagar.</summary>
public class Pago : BaseEntity
{
    public int CuentaPorPagarId { get; set; }
    public CuentaPorPagar? CuentaPorPagar { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public decimal Monto { get; set; }
    public string MedioPago { get; set; } = "EFECTIVO"; // EFECTIVO, TRANSFERENCIA, CHEQUE
    public string? Referencia { get; set; }
}
