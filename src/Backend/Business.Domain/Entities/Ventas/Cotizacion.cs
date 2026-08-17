namespace Business.Domain.Entities.Ventas;

using Business.Domain.Common;
using Business.Domain.Entities.Maestros;

public class Cotizacion : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public DateTime FechaVencimiento { get; set; }
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public string Estado { get; set; } = "BORRADOR"; // BORRADOR, ENVIADA, APROBADA, RECHAZADA
    public decimal SubTotal { get; set; }
    public decimal Igv { get; set; }
    public decimal Total { get; set; }
    public string? Observaciones { get; set; }
    public ICollection<CotizacionDetalle> Detalles { get; set; } = new List<CotizacionDetalle>();
}
