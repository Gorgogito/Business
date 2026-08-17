namespace Business.Application.Interfaces;

/// <summary>Acceso a parámetros de configuración del sistema (IGV, moneda, etc.).</summary>
public interface IParametroService
{
    /// <summary>Tasa de IGV como fracción (p. ej. 0.18). Toma el parámetro "IGV" o 0.18 por defecto.</summary>
    Task<decimal> GetIgvRateAsync(CancellationToken ct = default);

    /// <summary>Remuneración Mínima Vital. Toma el parámetro "RMV" o 1025 por defecto.</summary>
    Task<decimal> GetRmvAsync(CancellationToken ct = default);

    /// <summary>
    /// Tope de remuneración asegurable para la prima de seguro AFP (invalidez/sobrevivencia).
    /// Solo limita la base de cálculo de la prima de seguro; el aporte al fondo (10%) y la
    /// comisión no tienen tope. Toma el parámetro "TOPE_ASEGURABLE_AFP" o 10878 por defecto
    /// (valor referencial SBS; configurable porque la SBS lo actualiza periódicamente).
    /// </summary>
    Task<decimal> GetTopeAseguradoAfpAsync(CancellationToken ct = default);
}
