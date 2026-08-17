namespace Business.Application.DTOs.Ventas;

public class CreateCotizacionDto
{
    public DateTime FechaVencimiento { get; set; }
    public int ClienteId { get; set; }
    public string? Observaciones { get; set; }
    public List<CreateCotizacionDetalleDto> Detalles { get; set; } = new();
}

public class CreateCotizacionDetalleDto
{
    public int ProductoId { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Descuento { get; set; }
}
