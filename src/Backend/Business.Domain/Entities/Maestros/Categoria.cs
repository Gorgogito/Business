namespace Business.Domain.Entities.Maestros;

using Business.Domain.Common;

public class Categoria : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
