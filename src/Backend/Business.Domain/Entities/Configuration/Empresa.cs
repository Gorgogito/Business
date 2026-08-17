namespace Business.Domain.Entities.Security;

using Business.Domain.Common;
using Business.Domain.Entities.Configuration;

public class Empresa : BaseEntity
{
    public string RazonSocial { get; set; } = string.Empty;
    public string RUC { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public ICollection<Sucursal> Sucursales { get; set; } = new List<Sucursal>();
    public ICollection<User> Users { get; set; } = new List<User>();
}
