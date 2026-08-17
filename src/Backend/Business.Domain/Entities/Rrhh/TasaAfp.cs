namespace Business.Domain.Entities.Rrhh;

using Business.Domain.Common;

/// <summary>
/// Tasas previsionales de una AFP (Administradora de Fondos de Pensiones). En Perú el aporte
/// obligatorio al fondo es 10%, pero la comisión sobre el flujo y la prima de seguro varían por
/// administradora. Catálogo nacional (no multiempresa). Reemplaza los porcentajes fijos de los
/// conceptos AFP_COMISION/AFP_SEGURO cuando el trabajador tiene una AFP registrada.
/// </summary>
public class TasaAfp : BaseEntity
{
    /// <summary>Nombre de la AFP (Integra, Prima, Profuturo, Habitat). Único.</summary>
    public string Nombre { get; set; } = string.Empty;
    /// <summary>Aporte obligatorio al fondo, fracción (0.10 = 10%).</summary>
    public decimal AporteFondo { get; set; } = 0.10m;
    /// <summary>Comisión sobre el flujo (remuneración), fracción.</summary>
    public decimal ComisionFlujo { get; set; }
    /// <summary>Prima de seguro de invalidez y sobrevivencia, fracción.</summary>
    public decimal PrimaSeguro { get; set; }
}
