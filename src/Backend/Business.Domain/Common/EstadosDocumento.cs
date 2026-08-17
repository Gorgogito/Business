namespace Business.Domain.Common;

/// <summary>
/// Constantes de estados y tipos de documento. Centralizarlas evita "strings mágicos"
/// dispersos y los errores de tipeo al compararlos o asignarlos.
/// </summary>
public static class EstadoCotizacion
{
    public const string Borrador = "BORRADOR";
    public const string Enviada = "ENVIADA";
    public const string Aprobada = "APROBADA";
    public const string Rechazada = "RECHAZADA";
}

public static class EstadoPedido
{
    public const string Pendiente = "PENDIENTE";
    public const string EnProceso = "EN_PROCESO";
    public const string Entregado = "ENTREGADO";
    public const string Facturado = "FACTURADO";
    public const string Cancelado = "CANCELADO";
}

public static class EstadoFactura
{
    public const string Emitida = "EMITIDA";
    public const string Pagada = "PAGADA";
    public const string Anulada = "ANULADA";
}

public static class EstadoOrdenCompra
{
    public const string Pendiente = "PENDIENTE";
    public const string Aprobada = "APROBADA";
    public const string Recibida = "RECIBIDA";
    public const string Parcial = "PARCIAL";
    public const string Cancelada = "CANCELADA";
}

public static class EstadoRecepcion
{
    public const string Completa = "COMPLETA";
    public const string Parcial = "PARCIAL";
}

public static class TipoComprobante
{
    public const string Factura = "FACTURA";
    public const string Boleta = "BOLETA";
}

public static class TipoNota
{
    public const string Credito = "CREDITO";
    public const string Debito = "DEBITO";
}

public static class EstadoNotaVenta
{
    public const string Emitida = "EMITIDA";
    public const string Anulada = "ANULADA";
}

public static class EstadoGuiaRemision
{
    public const string Emitida = "EMITIDA";
    public const string Anulada = "ANULADA";
}

public static class MotivoTraslado
{
    public const string Venta = "VENTA";
    public const string Traslado = "TRASLADO";
    public const string Devolucion = "DEVOLUCION";
}

public static class EstadoCuentaPorCobrar
{
    public const string Pendiente = "PENDIENTE";
    public const string Parcial = "PARCIAL";
    public const string Pagada = "PAGADA";
    public const string Anulada = "ANULADA";
}

public static class EstadoCuentaPorPagar
{
    public const string Pendiente = "PENDIENTE";
    public const string Parcial = "PARCIAL";
    public const string Pagada = "PAGADA";
    public const string Anulada = "ANULADA";
}
