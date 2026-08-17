namespace Business.Domain.Entities.Inventario;

using Business.Domain.Common;

public class Almacen : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Ubicacion { get; set; }
    public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    public ICollection<MovimientoInventario> Movimientos { get; set; } = new List<MovimientoInventario>();
}
