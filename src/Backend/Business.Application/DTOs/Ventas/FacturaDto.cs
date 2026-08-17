namespace Business.Application.DTOs.Ventas;

public class FacturaDto
{
    public int Id { get; set; }
    public string Serie { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int ClienteId { get; set; }
    public string? ClienteNombre { get; set; }
    public int? PedidoId { get; set; }
    public int AlmacenId { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string TipoDocumento { get; set; } = string.Empty;
    public decimal SubTotal { get; set; }
    public decimal Igv { get; set; }
    public decimal Total { get; set; }
    public string? Observaciones { get; set; }
    public List<FacturaDetalleDto> Detalles { get; set; } = new();
}

public class FacturaDetalleDto
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Descuento { get; set; }
    public decimal SubTotal { get; set; }
}
