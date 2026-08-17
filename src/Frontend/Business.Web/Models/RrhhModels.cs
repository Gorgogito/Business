namespace Business.Web.Models;

public class TrabajadorModel
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string TipoDocumento { get; set; } = "DNI";
    public string NumeroDocumento { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string ApellidoPaterno { get; set; } = string.Empty;
    public string ApellidoMaterno { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public DateTime? FechaNacimiento { get; set; }
    public string? Sexo { get; set; }
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string Cargo { get; set; } = string.Empty;
    public DateTime FechaIngreso { get; set; } = DateTime.Now;
    public DateTime? FechaCese { get; set; }
    public string TipoContrato { get; set; } = "INDEFINIDO";
    public decimal SueldoBasico { get; set; }
    public bool TieneAsignacionFamiliar { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string RegimenPension { get; set; } = "ONP";
    public string? AfpNombre { get; set; }
    public string? Cuspp { get; set; }
}

public class ConceptoPlanillaModel
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = "INGRESO";
    public string MetodoCalculo { get; set; } = "MANUAL";
    public decimal? Porcentaje { get; set; }
    public decimal? MontoFijo { get; set; }
    public bool AfectaAfp { get; set; }
    public bool AfectaEssalud { get; set; }
    public bool EsSistema { get; set; }
    public int Orden { get; set; }
}

public class TasaAfpModel
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal AporteFondo { get; set; } = 0.10m;
    public decimal ComisionFlujo { get; set; }
    public decimal PrimaSeguro { get; set; }
}

public class PlanillaModel
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
    public List<PlanillaBoletaModel> Boletas { get; set; } = new();
}

public class PlanillaBoletaModel
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
    public List<PlanillaConceptoModel> Conceptos { get; set; } = new();
}

public class PlanillaConceptoModel
{
    public string ConceptoCodigo { get; set; } = string.Empty;
    public string ConceptoNombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public decimal Monto { get; set; }
}

public class BeneficioSocialModel
{
    public int Id { get; set; }
    public int TrabajadorId { get; set; }
    public string? TrabajadorCodigo { get; set; }
    public string? TrabajadorNombre { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Periodo { get; set; } = string.Empty;
    public DateTime FechaCalculo { get; set; }
    public decimal RemuneracionComputable { get; set; }
    public decimal MesesComputables { get; set; }
    public decimal Monto { get; set; }
    public decimal BonificacionExtraordinaria { get; set; }
    public decimal TotalPagar => Monto + BonificacionExtraordinaria;
    public string? Observacion { get; set; }
    public string EstadoPago { get; set; } = string.Empty;
    public DateTime? FechaPago { get; set; }
    public string? MedioPago { get; set; }
}
