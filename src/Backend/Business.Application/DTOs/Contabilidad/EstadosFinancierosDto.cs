namespace Business.Application.DTOs.Contabilidad;

public class EstadoResultadosDto
{
    public DateTime Desde { get; set; }
    public DateTime Hasta { get; set; }
    public decimal Ingresos { get; set; }
    public decimal CostoVentas { get; set; }
    public decimal UtilidadBruta { get; set; }
    public decimal Gastos { get; set; }
    public decimal UtilidadNeta { get; set; }
    public List<EstadoLineaDto> Detalle { get; set; } = new();
}

public class BalanceGeneralDto
{
    public DateTime Fecha { get; set; }
    public decimal TotalActivo { get; set; }
    public decimal TotalPasivo { get; set; }
    public decimal TotalPatrimonio { get; set; }
    public decimal UtilidadEjercicio { get; set; }
    public decimal TotalPasivoPatrimonio { get; set; }
    public bool Cuadra { get; set; }
    public List<EstadoLineaDto> Activos { get; set; } = new();
    public List<EstadoLineaDto> Pasivos { get; set; } = new();
    public List<EstadoLineaDto> Patrimonio { get; set; } = new();
}

public class EstadoLineaDto
{
    public string Grupo { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public decimal Monto { get; set; }
}
