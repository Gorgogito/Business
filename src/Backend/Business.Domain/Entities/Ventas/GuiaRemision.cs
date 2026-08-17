namespace Business.Domain.Entities.Ventas;

using Business.Domain.Common;
using Business.Domain.Entities.Maestros;

/// <summary>
/// Guía de remisión: documento que sustenta el traslado físico de mercadería. Puede
/// ligarse a una factura. Es documental (no altera el stock, que ya movió la factura).
/// </summary>
public class GuiaRemision : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public string Serie { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public DateTime FechaTraslado { get; set; }
    public int? FacturaId { get; set; }
    public Factura? Factura { get; set; }
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public string DireccionPartida { get; set; } = string.Empty;
    public string DireccionLlegada { get; set; } = string.Empty;
    public string? Transportista { get; set; }
    public string? TransportistaRuc { get; set; }
    public string? Placa { get; set; }
    public string Motivo { get; set; } = "VENTA"; // VENTA, TRASLADO, DEVOLUCION
    public string Estado { get; set; } = "EMITIDA"; // EMITIDA, ANULADA
    public ICollection<GuiaRemisionDetalle> Detalles { get; set; } = new List<GuiaRemisionDetalle>();
}
