namespace Business.Domain.Common;

public static class RegimenPension
{
    public const string Afp = "AFP";
    public const string Onp = "ONP";
}

public static class EstadoTrabajador
{
    public const string Activo = "ACTIVO";
    public const string Cesado = "CESADO";
}

public static class TipoContrato
{
    public const string Indefinido = "INDEFINIDO";
    public const string PlazoFijo = "PLAZO_FIJO";
    public const string TiempoParcial = "TIEMPO_PARCIAL";
}

public static class TipoConcepto
{
    public const string Ingreso = "INGRESO";     // suma a la remuneración
    public const string Descuento = "DESCUENTO"; // resta al trabajador (AFP/ONP, adelantos)
    public const string Aporte = "APORTE";       // costo del empleador (EsSalud), no afecta el neto
}

public static class MetodoCalculo
{
    public const string Fijo = "FIJO";           // monto fijo
    public const string Porcentual = "PORCENTUAL"; // % sobre la base imponible
    public const string Manual = "MANUAL";       // se ingresa al procesar la planilla
}

public static class EstadoPlanilla
{
    public const string Procesada = "PROCESADA";
    public const string Anulada = "ANULADA";
}

public static class TipoBeneficio
{
    public const string Cts = "CTS";                     // compensación por tiempo de servicios
    public const string Gratificacion = "GRATIFICACION"; // Fiestas Patrias / Navidad
    public const string Vacaciones = "VACACIONES";       // remuneración vacacional
}

public static class EstadoPagoBeneficio
{
    public const string Pendiente = "PENDIENTE";
    public const string Pagado = "PAGADO";
}
