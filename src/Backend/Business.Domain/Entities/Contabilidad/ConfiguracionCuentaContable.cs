namespace Business.Domain.Entities.Contabilidad;

using Business.Domain.Common;

/// <summary>
/// Asigna un concepto contable (p. ej. VENTA_INGRESO, PLANILLA_APORTES) a una cuenta del plan
/// de la empresa. Los asientos automáticos resuelven sus códigos de cuenta a través de esta
/// configuración en vez de tenerlos hardcodeados; si un concepto no está configurado se usa el
/// código por defecto de <see cref="ConceptosContables.Defaults"/>.
/// </summary>
public class ConfiguracionCuentaContable : BaseEntity, ITenantEntity
{
    public int EmpresaId { get; set; } = 1;

    /// <summary>Clave del concepto (ver <see cref="ConceptosContables"/>).</summary>
    public string Concepto { get; set; } = string.Empty;
    public string Modulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    public int CuentaContableId { get; set; }
    public CuentaContable CuentaContable { get; set; } = null!;
}
