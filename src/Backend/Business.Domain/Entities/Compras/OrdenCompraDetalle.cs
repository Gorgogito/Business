namespace Business.Domain.Entities.Compras;

using Business.Domain.Common;
using Business.Domain.Entities.Maestros;

public class OrdenCompraDetalle : BaseEntity
{
    public int OrdenCompraId { get; set; }
    public OrdenCompra? OrdenCompra { get; set; }
    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public decimal Cantidad { get; set; }
    public decimal CantidadRecibida { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal SubTotal { get; set; }
}
