namespace Business.Domain.Entities.Configuration;

using Business.Domain.Common;
using Business.Domain.Entities.Security;

public class Sucursal : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public int EmpresaId { get; set; }
    public Empresa? Empresa { get; set; }
}
