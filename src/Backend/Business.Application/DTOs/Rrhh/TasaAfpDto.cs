namespace Business.Application.DTOs.Rrhh;

public class TasaAfpDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal AporteFondo { get; set; }
    public decimal ComisionFlujo { get; set; }
    public decimal PrimaSeguro { get; set; }
}

public class CreateTasaAfpDto
{
    public string Nombre { get; set; } = string.Empty;
    public decimal AporteFondo { get; set; } = 0.10m;
    public decimal ComisionFlujo { get; set; }
    public decimal PrimaSeguro { get; set; }
}
