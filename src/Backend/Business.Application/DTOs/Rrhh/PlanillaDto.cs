namespace Business.Application.DTOs.Rrhh;

public class PlanillaDto
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public int Anio { get; set; }
    public int Mes { get; set; }
    public DateTime FechaProceso { get; set; }
    public string Estado { get; set; } = string.Empty;
    public decimal TotalIngresos { get; set; }
    public decimal TotalDescuentos { get; set; }
    public decimal TotalAportes { get; set; }
    public decimal TotalNeto { get; set; }
    public List<PlanillaBoletaDto> Boletas { get; set; } = new();
}

public class PlanillaBoletaDto
{
    public int Id { get; set; }
    public int TrabajadorId { get; set; }
    public string? TrabajadorCodigo { get; set; }
    public string? TrabajadorNombre { get; set; }
    public decimal SueldoBasico { get; set; }
    public string RegimenPension { get; set; } = string.Empty;
    public decimal TotalIngresos { get; set; }
    public decimal TotalDescuentos { get; set; }
    public decimal TotalAportes { get; set; }
    public decimal NetoPagar { get; set; }
    public List<PlanillaConceptoDto> Conceptos { get; set; } = new();
}

public class PlanillaConceptoDto
{
    public string ConceptoCodigo { get; set; } = string.Empty;
    public string ConceptoNombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public decimal Monto { get; set; }
}

public class ProcesarPlanillaDto
{
    public int Anio { get; set; }
    public int Mes { get; set; }
}
