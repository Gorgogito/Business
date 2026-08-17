namespace Business.Application.DTOs.Maestros;

public class CreateProductoDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal PrecioCompra { get; set; }
    public decimal PrecioVenta { get; set; }
    public string Unidad { get; set; } = "UND";
    public int CategoriaId { get; set; }
}
