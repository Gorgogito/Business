namespace Business.Domain.Entities.Compras;

using Business.Domain.Common;
using Business.Domain.Entities.Maestros;

public class RecepcionCompraDetalle : BaseEntity
{
    public int RecepcionCompraId { get; set; }
    public RecepcionCompra? RecepcionCompra { get; set; }
    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public decimal CantidadEsperada { get; set; }
    public decimal CantidadRecibida { get; set; }
    public decimal PrecioUnitario { get; set; }
}
