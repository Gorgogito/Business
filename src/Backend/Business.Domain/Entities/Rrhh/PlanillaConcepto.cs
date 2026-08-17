namespace Business.Domain.Entities.Rrhh;

using Business.Domain.Common;

/// <summary>Línea de un concepto aplicado en la boleta (ingreso, descuento o aporte).</summary>
public class PlanillaConcepto : BaseEntity
{
    public int PlanillaDetalleId { get; set; }
    public PlanillaDetalle? PlanillaDetalle { get; set; }
    public string ConceptoCodigo { get; set; } = string.Empty;
    public string ConceptoNombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty; // INGRESO, DESCUENTO, APORTE
    public decimal Monto { get; set; }
}
