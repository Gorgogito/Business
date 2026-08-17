namespace Business.Application.DTOs.Rrhh;

public class ConceptoPlanillaDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string MetodoCalculo { get; set; } = string.Empty;
    public decimal? Porcentaje { get; set; }
    public decimal? MontoFijo { get; set; }
    public bool AfectaAfp { get; set; }
    public bool AfectaEssalud { get; set; }
    public bool EsSistema { get; set; }
    public int Orden { get; set; }
}

public class CreateConceptoPlanillaDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = "INGRESO";
    public string MetodoCalculo { get; set; } = "MANUAL";
    public decimal? Porcentaje { get; set; }
    public decimal? MontoFijo { get; set; }
    public bool AfectaAfp { get; set; }
    public bool AfectaEssalud { get; set; }
    public int Orden { get; set; }
}
