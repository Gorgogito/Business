namespace Business.Domain.Entities.Finanzas;

using Business.Domain.Common;
using Business.Domain.Entities.Maestros;
using Business.Domain.Entities.Ventas;

/// <summary>
/// Obligación de cobro generada al emitir una factura. Lleva el saldo pendiente y los
/// cobros aplicados, permitiendo controlar la cartera y la morosidad.
/// </summary>
public class CuentaPorCobrar : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public int FacturaId { get; set; }
    public Factura? Factura { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public decimal MontoTotal { get; set; }
    public decimal SaldoPendiente { get; set; }
    public string Estado { get; set; } = "PENDIENTE"; // PENDIENTE, PARCIAL, PAGADA, ANULADA
    public ICollection<Cobro> Cobros { get; set; } = new List<Cobro>();
}
