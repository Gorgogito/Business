namespace Business.Domain.Entities.Produccion;

using Business.Domain.Common;
using Business.Domain.Entities.Maestros;

/// <summary>Insumo consumido por una orden de fabricación, valorizado al costo promedio.</summary>
public class OrdenFabricacionDetalle : BaseEntity
{
    public int OrdenFabricacionId { get; set; }
    public OrdenFabricacion? OrdenFabricacion { get; set; }
    public int InsumoId { get; set; }
    public Producto? Insumo { get; set; }
    public decimal Cantidad { get; set; }
    public decimal CostoUnitario { get; set; }  // costo promedio del insumo al consumir
    public decimal CostoTotal { get; set; }
}
