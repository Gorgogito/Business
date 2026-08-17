namespace Business.Application.DTOs.Inventario;

public class CreateMovimientoDto
{
    public string Tipo { get; set; } = string.Empty;
    public int ProductoId { get; set; }
    public int AlmacenId { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public string? Referencia { get; set; }
    public string? Observacion { get; set; }
}
