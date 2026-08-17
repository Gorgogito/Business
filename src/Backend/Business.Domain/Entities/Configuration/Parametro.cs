namespace Business.Domain.Entities.Configuration;

using Business.Domain.Common;

public class Parametro : BaseEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Modulo { get; set; } = string.Empty;
}
