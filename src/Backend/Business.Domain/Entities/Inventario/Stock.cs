namespace Business.Domain.Entities.Inventario;

using Business.Domain.Common;
using Business.Domain.Entities.Maestros;

public class Stock : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public int AlmacenId { get; set; }
    public Almacen? Almacen { get; set; }
    public decimal CantidadActual { get; set; }
    public decimal StockMinimo { get; set; }
    public decimal StockMaximo { get; set; }
    /// <summary>Costo promedio ponderado móvil de la unidad en existencia.</summary>
    public decimal CostoPromedio { get; set; }
}
