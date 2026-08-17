namespace Business.Application.DTOs.Compras;

public class OrdenCompraDto
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public DateTime FechaEntrega { get; set; }
    public int ProveedorId { get; set; }
    public string? ProveedorNombre { get; set; }
    public string Estado { get; set; } = string.Empty;
    public decimal SubTotal { get; set; }
    public decimal Igv { get; set; }
    public decimal Total { get; set; }
    public string? Observaciones { get; set; }
    public List<OrdenCompraDetalleDto> Detalles { get; set; } = new();
}

public class OrdenCompraDetalleDto
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public decimal Cantidad { get; set; }
    public decimal CantidadRecibida { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal SubTotal { get; set; }
}
