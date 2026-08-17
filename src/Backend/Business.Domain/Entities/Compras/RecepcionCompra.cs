namespace Business.Domain.Entities.Compras;

using Business.Domain.Common;

public class RecepcionCompra : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public int OrdenCompraId { get; set; }
    public OrdenCompra? OrdenCompra { get; set; }
    public int AlmacenId { get; set; } = 1; // Almacén al que ingresa la mercadería
    public string Estado { get; set; } = "COMPLETA"; // PARCIAL, COMPLETA
    public string? Observaciones { get; set; }
    public ICollection<RecepcionCompraDetalle> Detalles { get; set; } = new List<RecepcionCompraDetalle>();
}
