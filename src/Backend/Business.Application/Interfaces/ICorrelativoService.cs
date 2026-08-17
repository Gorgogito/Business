namespace Business.Application.Interfaces;

/// <summary>
/// Genera números de documento correlativos y únicos de forma segura ante concurrencia.
/// Reemplaza los esquemas basados en Count()+1 o Random(), que podían producir duplicados.
/// </summary>
public interface ICorrelativoService
{
    /// <summary>
    /// Reserva y devuelve el siguiente número formateado (prefijo + número con relleno de ceros)
    /// para el tipo de documento y serie indicados. La operación es atómica.
    /// </summary>
    /// <param name="tipoDocumento">Tipo lógico: COTIZACION, PEDIDO, FACTURA, BOLETA, ORDEN_COMPRA, RECEPCION.</param>
    /// <param name="serie">Serie del documento: COT, PED, F001, B001, OC, REC.</param>
    /// <param name="empresaId">Empresa (multiempresa). Por defecto 1.</param>
    Task<string> SiguienteAsync(string tipoDocumento, string serie, int empresaId = 1, CancellationToken ct = default);
}
