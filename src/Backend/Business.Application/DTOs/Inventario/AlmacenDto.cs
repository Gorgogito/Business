namespace Business.Application.DTOs.Inventario;

public class AlmacenDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Ubicacion { get; set; }
    public bool IsActive { get; set; }
}
