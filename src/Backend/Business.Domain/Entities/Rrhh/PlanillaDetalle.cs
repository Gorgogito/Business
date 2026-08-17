namespace Business.Domain.Entities.Rrhh;

using Business.Domain.Common;

/// <summary>Boleta de pago de un trabajador dentro de una planilla mensual.</summary>
public class PlanillaDetalle : BaseEntity
{
    public int PlanillaId { get; set; }
    public Planilla? Planilla { get; set; }
    public int TrabajadorId { get; set; }
    public Trabajador? Trabajador { get; set; }
    public decimal SueldoBasico { get; set; }
    public string RegimenPension { get; set; } = string.Empty;
    public decimal TotalIngresos { get; set; }
    public decimal TotalDescuentos { get; set; }
    public decimal TotalAportes { get; set; }
    public decimal NetoPagar { get; set; }
    public ICollection<PlanillaConcepto> Conceptos { get; set; } = new List<PlanillaConcepto>();
}
