namespace Business.Application.DTOs.Maestros;

public class ProductoDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal PrecioCompra { get; set; }
    public decimal PrecioVenta { get; set; }
    public string Unidad { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public string? CategoriaNombre { get; set; }
    public bool IsActive { get; set; }
}
