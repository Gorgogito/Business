namespace Business.Application.Interfaces;

using Business.Application.DTOs.Contabilidad;

/// <summary>
/// Resuelve conceptos contables (ver <c>ConceptosContables</c>) a cuentas del plan de la
/// empresa actual, para que los asientos automáticos (venta, compra, cobranza, pago, planilla,
/// producción) no tengan códigos de cuenta hardcodeados.
/// </summary>
public interface IConfiguracionContableService
{
    /// <summary>Catálogo completo de conceptos con su configuración actual (personalizada o por defecto).</summary>
    Task<IEnumerable<ConfiguracionCuentaContableDto>> GetAllAsync();

    /// <summary>Configura (o reconfigura) la cuenta de un concepto para la empresa actual.</summary>
    Task<ConfiguracionCuentaContableDto> ConfigurarAsync(string concepto, int cuentaContableId, string userName);

    /// <summary>
    /// Mapa concepto -> código de cuenta para la empresa actual, completado con los valores por
    /// defecto para los conceptos que la empresa no configuró explícitamente.
    /// </summary>
    Task<IReadOnlyDictionary<string, string>> ObtenerMapaAsync(CancellationToken ct = default);
}
