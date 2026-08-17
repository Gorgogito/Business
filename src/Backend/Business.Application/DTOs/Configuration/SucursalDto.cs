namespace Business.Application.DTOs.Configuration;

public class SucursalDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public int EmpresaId { get; set; }
    public string? EmpresaNombre { get; set; }
    public bool IsActive { get; set; }
}
