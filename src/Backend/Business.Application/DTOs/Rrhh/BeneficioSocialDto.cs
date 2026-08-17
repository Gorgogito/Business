namespace Business.Application.DTOs.Rrhh;

public class BeneficioSocialDto
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
    /// <summary>Monto total a pagar (Monto + bonificación extraordinaria).</summary>
    public decimal TotalPagar => Monto + BonificacionExtraordinaria;
    public string? Observacion { get; set; }
    public string EstadoPago { get; set; } = string.Empty;
    public DateTime? FechaPago { get; set; }
    public string? MedioPago { get; set; }
}

public class RegistrarPagoBeneficioDto
{
    public string MedioPago { get; set; } = "EFECTIVO";
}
