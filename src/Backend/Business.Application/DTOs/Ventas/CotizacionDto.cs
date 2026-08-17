namespace Business.Application.DTOs.Ventas;

public class CotizacionDto
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public int ClienteId { get; set; }
    public string? ClienteNombre { get; set; }
    public string Estado { get; set; } = string.Empty;
    public decimal SubTotal { get; set; }
    public decimal Igv { get; set; }
    public decimal Total { get; set; }
    public string? Observaciones { get; set; }
    public List<CotizacionDetalleDto> Detalles { get; set; } = new();
}

public class CotizacionDetalleDto
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Descuento { get; set; }
    public decimal SubTotal { get; set; }
}
