namespace Business.Domain.Entities.Contabilidad;

using Business.Domain.Common;

/// <summary>
/// Cuenta del Plan Contable General Empresarial (PCGE). El código es jerárquico
/// (2 dígitos = cuenta, 3 = subcuenta, 4+ = divisionaria). Solo las cuentas de detalle
/// (EsMovimiento = true) admiten asientos.
/// </summary>
public class CuentaContable : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del plan (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    /// <summary>ACTIVO, PASIVO, PATRIMONIO, INGRESO, GASTO, COSTO.</summary>
    public string Clase { get; set; } = string.Empty;
    /// <summary>DEUDORA (aumenta por el debe) o ACREEDORA (aumenta por el haber).</summary>
    public string Naturaleza { get; set; } = string.Empty;
    /// <summary>Nivel jerárquico según la longitud del código.</summary>
    public int Nivel { get; set; }
    /// <summary>Indica si la cuenta admite movimientos (asientos) o solo agrupa.</summary>
    public bool EsMovimiento { get; set; }
}
