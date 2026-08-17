namespace Business.Domain.Entities.Produccion;

using Business.Domain.Common;
using Business.Domain.Entities.Maestros;

/// <summary>
/// Receta o fórmula de producción (lista de materiales): define los insumos y cantidades
/// necesarios para producir una cantidad base de un producto terminado.
/// </summary>
public class Receta : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public string Codigo { get; set; } = string.Empty;
    public int ProductoId { get; set; }         // producto terminado
    public Producto? Producto { get; set; }
    public string? Descripcion { get; set; }
    /// <summary>Rendimiento base: cantidad de producto que rinde la receta.</summary>
    public decimal CantidadProducida { get; set; } = 1;
    public string Estado { get; set; } = "ACTIVA"; // ACTIVA, INACTIVA
    public ICollection<RecetaDetalle> Detalles { get; set; } = new List<RecetaDetalle>();
}
