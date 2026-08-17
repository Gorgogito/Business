namespace Business.Web.Models;

public class CuentaContableModel
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Clase { get; set; } = string.Empty;
    public string Naturaleza { get; set; } = string.Empty;
    public int Nivel { get; set; }
    public bool EsMovimiento { get; set; }
}

public class AsientoContableModel
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public string Glosa { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string? Referencia { get; set; }
    public decimal TotalDebe { get; set; }
    public decimal TotalHaber { get; set; }
    public string Estado { get; set; } = string.Empty;
    public List<AsientoDetalleModel> Detalles { get; set; } = new();
}

public class AsientoDetalleModel
{
    public int CuentaContableId { get; set; }
    public string? CuentaCodigo { get; set; }
    public string? CuentaNombre { get; set; }
    public decimal Debe { get; set; }
    public decimal Haber { get; set; }
    public string? Glosa { get; set; }
}

public class LibroMayorModel
{
    public string CuentaCodigo { get; set; } = string.Empty;
    public string CuentaNombre { get; set; } = string.Empty;
    public string Naturaleza { get; set; } = string.Empty;
    public DateTime Desde { get; set; }
    public DateTime Hasta { get; set; }
    public decimal TotalDebe { get; set; }
    public decimal TotalHaber { get; set; }
    public decimal SaldoFinal { get; set; }
    public List<MayorLineaModel> Movimientos { get; set; } = new();
}

public class MayorLineaModel
{
    public DateTime Fecha { get; set; }
    public string AsientoNumero { get; set; } = string.Empty;
    public string? Glosa { get; set; }
    public decimal Debe { get; set; }
    public decimal Haber { get; set; }
    public decimal SaldoAcumulado { get; set; }
}

public class BalanceComprobacionModel
{
    public DateTime Desde { get; set; }
    public DateTime Hasta { get; set; }
    public decimal TotalDebe { get; set; }
    public decimal TotalHaber { get; set; }
    public decimal TotalSaldoDeudor { get; set; }
    public decimal TotalSaldoAcreedor { get; set; }
    public bool Cuadra { get; set; }
    public List<BalanceLineaModel> Cuentas { get; set; } = new();
}

public class BalanceLineaModel
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public decimal Debe { get; set; }
    public decimal Haber { get; set; }
    public decimal SaldoDeudor { get; set; }
    public decimal SaldoAcreedor { get; set; }
}

// EstadoResultadosModel y EstadoLineaModel ya están definidos en AnaliticaModels.cs (reutilizados aquí).

public class BalanceGeneralModel
{
    public DateTime Fecha { get; set; }
    public decimal TotalActivo { get; set; }
    public decimal TotalPasivo { get; set; }
    public decimal TotalPatrimonio { get; set; }
    public decimal UtilidadEjercicio { get; set; }
    public decimal TotalPasivoPatrimonio { get; set; }
    public bool Cuadra { get; set; }
    public List<EstadoLineaModel> Activos { get; set; } = new();
    public List<EstadoLineaModel> Pasivos { get; set; } = new();
    public List<EstadoLineaModel> Patrimonio { get; set; } = new();
}

public class ConfiguracionCuentaContableModel
{
    public string Concepto { get; set; } = string.Empty;
    public string Modulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string CuentaCodigoDefecto { get; set; } = string.Empty;
    public int? CuentaContableId { get; set; }
    public string? CuentaCodigo { get; set; }
    public string? CuentaNombre { get; set; }
    public bool EsPersonalizado { get; set; }
}
