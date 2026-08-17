namespace Business.Application.DTOs.Ventas;

public class CreatePedidoDto
{
    public int ClienteId { get; set; }
    public int? CotizacionId { get; set; }
    public string? Observaciones { get; set; }
    public List<CreatePedidoDetalleDto> Detalles { get; set; } = new();
}

public class CreatePedidoDetalleDto
{
    public int ProductoId { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Descuento { get; set; }
}
