namespace Business.Application.DTOs.Contabilidad;

public class ConfiguracionCuentaContableDto
{
    public string Concepto { get; set; } = string.Empty;
    public string Modulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string CuentaCodigoDefecto { get; set; } = string.Empty;
    public int? CuentaContableId { get; set; }
    public string? CuentaCodigo { get; set; }
    public string? CuentaNombre { get; set; }
    /// <summary>true si la empresa configuró una cuenta propia para este concepto; false si usa el código por defecto.</summary>
    public bool EsPersonalizado { get; set; }
}

public class ConfigurarCuentaContableDto
{
    public int CuentaContableId { get; set; }
}
