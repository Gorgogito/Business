namespace Business.Application.DTOs.Finanzas;

public class CuentaPorCobrarDto
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string? ClienteNombre { get; set; }
    public int FacturaId { get; set; }
    public string? FacturaNumero { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public decimal MontoTotal { get; set; }
    public decimal SaldoPendiente { get; set; }
    public string Estado { get; set; } = string.Empty;
    public bool Vencida { get; set; }
    public List<CobroDto> Cobros { get; set; } = new();
}

public class CobroDto
{
    public int Id { get; set; }
    public int CuentaPorCobrarId { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Monto { get; set; }
    public string MedioPago { get; set; } = string.Empty;
    public string? Referencia { get; set; }
}

public class CreateCobroDto
{
    public decimal Monto { get; set; }
    public string MedioPago { get; set; } = "EFECTIVO";
    public string? Referencia { get; set; }
}
