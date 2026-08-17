namespace Business.Application.DTOs.Configuration;

public class CreateSucursalDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public int EmpresaId { get; set; }
}
