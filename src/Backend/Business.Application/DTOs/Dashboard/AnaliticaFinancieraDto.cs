namespace Business.Application.DTOs.Dashboard;

using Business.Application.DTOs.Contabilidad;

/// <summary>
/// Analítica gerencial financiera: aging (antigüedad de saldos) de la cartera por cobrar y por
/// pagar a una fecha de corte, más el estado de resultados del período con drill por cuenta.
/// </summary>
public class AnaliticaFinancieraDto
{
    public DateTime Desde { get; set; }
    public DateTime Hasta { get; set; }

    public AgingDto CarteraCobrar { get; set; } = new();
    public AgingDto CarteraPagar { get; set; } = new();

    /// <summary>Utilidad del período con drill a las cuentas de ingreso/costo/gasto.</summary>
    public EstadoResultadosDto EstadoResultados { get; set; } = new();
}

/// <summary>Antigüedad de saldos: total y apertura por tramos de días vencidos.</summary>
public class AgingDto
{
    public decimal Total { get; set; }
    public int Documentos { get; set; }
    public List<AgingTramoDto> Tramos { get; set; } = new();
    public List<CarteraItemDto> Detalle { get; set; } = new();
}

public class AgingTramoDto
{
    public string Tramo { get; set; } = string.Empty; // Vigente, 1-30, 31-60, 61-90, >90
    public decimal Monto { get; set; }
    public int Documentos { get; set; }
}

public class CarteraItemDto
{
    public string Contraparte { get; set; } = string.Empty;
    public string Documento { get; set; } = string.Empty;
    public DateTime FechaVencimiento { get; set; }
    public int DiasVencido { get; set; }
    public decimal Saldo { get; set; }
    public string Tramo { get; set; } = string.Empty;
}
