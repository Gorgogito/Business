namespace Business.Domain.Entities.Finanzas;

using Business.Domain.Common;
using Business.Domain.Entities.Maestros;
using Business.Domain.Entities.Compras;

/// <summary>
/// Obligación de pago a un proveedor, generada al recepcionar mercadería. Lleva el saldo
/// pendiente y los pagos aplicados, permitiendo controlar la deuda y su vencimiento.
/// </summary>
public class CuentaPorPagar : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public int ProveedorId { get; set; }
    public Proveedor? Proveedor { get; set; }
    public int RecepcionCompraId { get; set; }
    public RecepcionCompra? RecepcionCompra { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public decimal MontoTotal { get; set; }
    public decimal SaldoPendiente { get; set; }
    public string Estado { get; set; } = "PENDIENTE"; // PENDIENTE, PARCIAL, PAGADA, ANULADA
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
