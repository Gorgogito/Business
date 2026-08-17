namespace Business.Application.DTOs.Contabilidad;

public class LibroMayorDto
{
    public string CuentaCodigo { get; set; } = string.Empty;
    public string CuentaNombre { get; set; } = string.Empty;
    public string Naturaleza { get; set; } = string.Empty;
    public DateTime Desde { get; set; }
    public DateTime Hasta { get; set; }
    public decimal TotalDebe { get; set; }
    public decimal TotalHaber { get; set; }
    public decimal SaldoFinal { get; set; }
    public List<MayorLineaDto> Movimientos { get; set; } = new();
}

public class MayorLineaDto
{
    public DateTime Fecha { get; set; }
    public string AsientoNumero { get; set; } = string.Empty;
    public string? Glosa { get; set; }
    public decimal Debe { get; set; }
    public decimal Haber { get; set; }
    public decimal SaldoAcumulado { get; set; }
}

public class BalanceComprobacionDto
{
    public DateTime Desde { get; set; }
    public DateTime Hasta { get; set; }
    public decimal TotalDebe { get; set; }
    public decimal TotalHaber { get; set; }
    public decimal TotalSaldoDeudor { get; set; }
    public decimal TotalSaldoAcreedor { get; set; }
    public bool Cuadra { get; set; }
    public List<BalanceLineaDto> Cuentas { get; set; } = new();
}

public class BalanceLineaDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public decimal Debe { get; set; }
    public decimal Haber { get; set; }
    public decimal SaldoDeudor { get; set; }
    public decimal SaldoAcreedor { get; set; }
}
