namespace Business.Domain.Entities.Compras;

using Business.Domain.Common;
using Business.Domain.Entities.Maestros;

public class OrdenCompra : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public DateTime FechaEntrega { get; set; }
    public int ProveedorId { get; set; }
    public Proveedor? Proveedor { get; set; }
    public string Estado { get; set; } = "PENDIENTE"; // PENDIENTE, APROBADA, RECIBIDA, CANCELADA
    public decimal SubTotal { get; set; }
    public decimal Igv { get; set; }
    public decimal Total { get; set; }
    public string? Observaciones { get; set; }
    public ICollection<OrdenCompraDetalle> Detalles { get; set; } = new List<OrdenCompraDetalle>();
    public ICollection<RecepcionCompra> Recepciones { get; set; } = new List<RecepcionCompra>();
}
