namespace Business.Domain.Entities.Configuration;

using Business.Domain.Common;

/// <summary>
/// Contador de numeración por tipo de documento y serie. Cada fila mantiene el
/// último número emitido, garantizando correlativos únicos y sin colisiones.
/// El incremento se realiza de forma atómica en base de datos (ver CorrelativoService).
/// </summary>
public class Correlativo : BaseEntity
{
    /// <summary>Tipo lógico de documento: COTIZACION, PEDIDO, FACTURA, BOLETA, ORDEN_COMPRA, RECEPCION.</summary>
    public string TipoDocumento { get; set; } = string.Empty;

    /// <summary>Serie del documento: COT, PED, F001, B001, OC, REC.</summary>
    public string Serie { get; set; } = string.Empty;

    /// <summary>Empresa a la que pertenece el correlativo (preparado para multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    /// <summary>Último número emitido. El siguiente documento usa UltimoNumero + 1.</summary>
    public int UltimoNumero { get; set; }

    /// <summary>Cantidad de dígitos con la que se rellena el número (p. ej. 8 => 00000001).</summary>
    public int Longitud { get; set; } = 8;

    /// <summary>Prefijo que antecede al número correlativo (p. ej. "COT-"). Vacío cuando la serie va aparte (facturas).</summary>
    public string Prefijo { get; set; } = string.Empty;
}
