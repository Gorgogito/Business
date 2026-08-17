namespace Business.Domain.Entities.Ventas;

using Business.Domain.Common;
using Business.Domain.Entities.Maestros;

public class FacturaDetalle : BaseEntity
{
    public int FacturaId { get; set; }
    public Factura? Factura { get; set; }
    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Descuento { get; set; }
    public decimal SubTotal { get; set; }
}
