namespace Business.Domain.Entities.Ventas;

using Business.Domain.Common;
using Business.Domain.Entities.Maestros;

public class Pedido : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public int? CotizacionId { get; set; }
    public Cotizacion? Cotizacion { get; set; }
    public string Estado { get; set; } = "PENDIENTE"; // PENDIENTE, EN_PROCESO, ENTREGADO, CANCELADO
    public decimal SubTotal { get; set; }
    public decimal Igv { get; set; }
    public decimal Total { get; set; }
    public string? Observaciones { get; set; }
    public ICollection<PedidoDetalle> Detalles { get; set; } = new List<PedidoDetalle>();
    public ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}
