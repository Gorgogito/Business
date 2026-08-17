namespace Business.Domain.Entities.Rrhh;

using Business.Domain.Common;

/// <summary>
/// Concepto de planilla: ingreso, descuento o aporte del empleador. Define cómo se calcula
/// (monto fijo, porcentaje sobre la base imponible, o ingreso manual) y si los ingresos
/// forman parte de la base afecta a AFP/ONP y EsSalud. Catálogo compartido (no multiempresa).
/// </summary>
public class ConceptoPlanilla : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del concepto (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;          // INGRESO, DESCUENTO, APORTE
    public string MetodoCalculo { get; set; } = string.Empty; // FIJO, PORCENTUAL, MANUAL
    public decimal? Porcentaje { get; set; }                  // fracción (0.13 = 13%)
    public decimal? MontoFijo { get; set; }
    /// <summary>Para ingresos: si integra la base afecta a AFP/ONP.</summary>
    public bool AfectaAfp { get; set; }
    /// <summary>Para ingresos: si integra la base afecta a EsSalud.</summary>
    public bool AfectaEssalud { get; set; }
    /// <summary>Concepto base del sistema (no se puede eliminar).</summary>
    public bool EsSistema { get; set; }
    public int Orden { get; set; }
}
