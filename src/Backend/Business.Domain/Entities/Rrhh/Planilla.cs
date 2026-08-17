namespace Business.Domain.Entities.Rrhh;

using Business.Domain.Common;

/// <summary>Planilla mensual de remuneraciones: cabecera del proceso de un período.</summary>
public class Planilla : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public string Numero { get; set; } = string.Empty;
    public int Anio { get; set; }
    public int Mes { get; set; }
    public DateTime FechaProceso { get; set; } = DateTime.UtcNow;
    public string Estado { get; set; } = "PROCESADA"; // PROCESADA, ANULADA
    public decimal TotalIngresos { get; set; }
    public decimal TotalDescuentos { get; set; }
    public decimal TotalAportes { get; set; }
    public decimal TotalNeto { get; set; }
    public ICollection<PlanillaDetalle> Detalles { get; set; } = new List<PlanillaDetalle>();
}
