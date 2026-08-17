namespace Business.Domain.Entities.Maestros;

using Business.Domain.Common;

public class Producto : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal PrecioCompra { get; set; }
    public decimal PrecioVenta { get; set; }
    public string Unidad { get; set; } = "UND";
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
    public ICollection<Inventario.Stock> Stocks { get; set; } = new List<Inventario.Stock>();
}
