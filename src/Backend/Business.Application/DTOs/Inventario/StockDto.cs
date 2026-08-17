namespace Business.Application.DTOs.Inventario;

public class StockDto
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public string? ProductoCodigo { get; set; }
    public int AlmacenId { get; set; }
    public string? AlmacenNombre { get; set; }
    public decimal CantidadActual { get; set; }
    public decimal StockMinimo { get; set; }
    public decimal StockMaximo { get; set; }
    public decimal CostoPromedio { get; set; }
    public decimal ValorInventario { get; set; }
}
