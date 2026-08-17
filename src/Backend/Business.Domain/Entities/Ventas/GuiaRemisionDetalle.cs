namespace Business.Domain.Entities.Ventas;

using Business.Domain.Common;
using Business.Domain.Entities.Maestros;

public class GuiaRemisionDetalle : BaseEntity
{
    public int GuiaRemisionId { get; set; }
    public GuiaRemision? GuiaRemision { get; set; }
    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public decimal Cantidad { get; set; }
    public string? Descripcion { get; set; }
}
