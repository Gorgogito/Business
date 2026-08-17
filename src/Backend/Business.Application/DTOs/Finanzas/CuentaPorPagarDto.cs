namespace Business.Application.DTOs.Finanzas;

public class CuentaPorPagarDto
{
    public int Id { get; set; }
    public int ProveedorId { get; set; }
    public string? ProveedorNombre { get; set; }
    public int RecepcionCompraId { get; set; }
    public string? RecepcionNumero { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public decimal MontoTotal { get; set; }
    public decimal SaldoPendiente { get; set; }
    public string Estado { get; set; } = string.Empty;
    public bool Vencida { get; set; }
    public List<PagoDto> Pagos { get; set; } = new();
}

public class PagoDto
{
    public int Id { get; set; }
    public int CuentaPorPagarId { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Monto { get; set; }
    public string MedioPago { get; set; } = string.Empty;
    public string? Referencia { get; set; }
}

public class CreatePagoDto
{
    public decimal Monto { get; set; }
    public string MedioPago { get; set; } = "EFECTIVO";
    public string? Referencia { get; set; }
}
