namespace Business.Domain.Entities.Ventas;

using Business.Domain.Common;
using Business.Domain.Entities.Maestros;

public class Factura : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public string Serie { get; set; } = "F001";
    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public int? PedidoId { get; set; }
    public Pedido? Pedido { get; set; }
    public int AlmacenId { get; set; } = 1; // Almacén desde el que se descuenta el stock
    public string Estado { get; set; } = "EMITIDA"; // EMITIDA, PAGADA, ANULADA
    public string TipoDocumento { get; set; } = "FACTURA"; // FACTURA, BOLETA
    public decimal SubTotal { get; set; }
    public decimal Igv { get; set; }
    public decimal Total { get; set; }
    public string? Observaciones { get; set; }
    public ICollection<FacturaDetalle> Detalles { get; set; } = new List<FacturaDetalle>();
}
