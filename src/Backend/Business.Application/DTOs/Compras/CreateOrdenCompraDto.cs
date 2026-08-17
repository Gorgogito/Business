namespace Business.Application.DTOs.Compras;

public class CreateOrdenCompraDto
{
    public DateTime FechaEntrega { get; set; }
    public int ProveedorId { get; set; }
    public string? Observaciones { get; set; }
    public List<CreateOrdenCompraDetalleDto> Detalles { get; set; } = new();
}

public class CreateOrdenCompraDetalleDto
{
    public int ProductoId { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}
