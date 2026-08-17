namespace Business.Application.DTOs.Maestros;

public class ProveedorDto
{
    public int Id { get; set; }
    public string RazonSocial { get; set; } = string.Empty;
    public string RUC { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string TipoDocumento { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
