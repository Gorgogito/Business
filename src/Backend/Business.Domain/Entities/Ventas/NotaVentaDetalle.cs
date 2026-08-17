namespace Business.Domain.Entities.Ventas;

using Business.Domain.Common;
using Business.Domain.Entities.Maestros;

public class NotaVentaDetalle : BaseEntity
{
    public int NotaVentaId { get; set; }
    public NotaVenta? NotaVenta { get; set; }
    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Descuento { get; set; }
    public decimal SubTotal { get; set; }
}
