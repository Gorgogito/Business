namespace Business.Application.DTOs.Inventario;

public class CreateAlmacenDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Ubicacion { get; set; }
}
