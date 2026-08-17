namespace Business.Domain.Entities.Produccion;

using Business.Domain.Common;
using Business.Domain.Entities.Maestros;

/// <summary>
/// Orden de fabricación: instrucción para producir N unidades de un producto según su
/// receta. Al procesarse consume la materia prima (salida valorizada), incorpora la mano
/// de obra (MOD) y los costos indirectos (CIF), y da de alta el producto terminado al
/// costo de producción (MP + MOD + CIF).
/// </summary>
public class OrdenFabricacion : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public string Numero { get; set; } = string.Empty;
    public int ProductoId { get; set; }         // producto terminado
    public Producto? Producto { get; set; }
    public int RecetaId { get; set; }
    public Receta? Receta { get; set; }
    public decimal CantidadProducir { get; set; }
    public int AlmacenId { get; set; } = 1;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string Estado { get; set; } = "PENDIENTE"; // PENDIENTE, TERMINADA, ANULADA
    public decimal CostoMateriaPrima { get; set; }
    public decimal CostoManoObra { get; set; }   // MOD
    public decimal CostoIndirecto { get; set; }  // CIF
    public decimal CostoTotal { get; set; }
    public decimal CostoUnitario { get; set; }
    public ICollection<OrdenFabricacionDetalle> Detalles { get; set; } = new List<OrdenFabricacionDetalle>();
}
