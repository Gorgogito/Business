namespace Business.Application.DTOs.Inventario;

public class MovimientoInventarioDto
{
    public int Id { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public int AlmacenId { get; set; }
    public string? AlmacenNombre { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal CostoUnitario { get; set; }
    public decimal CostoTotal { get; set; }
    public string? Referencia { get; set; }
    public string? Observacion { get; set; }
    public DateTime FechaMovimiento { get; set; }
}
