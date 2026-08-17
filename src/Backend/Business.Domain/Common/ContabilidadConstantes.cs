namespace Business.Domain.Common;

public static class ClaseCuenta
{
    public const string Activo = "ACTIVO";
    public const string Pasivo = "PASIVO";
    public const string Patrimonio = "PATRIMONIO";
    public const string Ingreso = "INGRESO";
    public const string Gasto = "GASTO";
    public const string Costo = "COSTO";
}

public static class NaturalezaCuenta
{
    public const string Deudora = "DEUDORA";
    public const string Acreedora = "ACREEDORA";

    /// <summary>Deriva la naturaleza (saldo normal) a partir de la clase de la cuenta.</summary>
    public static string DesdeClase(string clase) => clase switch
    {
        ClaseCuenta.Activo or ClaseCuenta.Gasto or ClaseCuenta.Costo => Deudora,
        _ => Acreedora
    };
}

public static class TipoAsiento
{
    public const string Diario = "DIARIO";
    public const string Venta = "VENTA";
    public const string Compra = "COMPRA";
    public const string Cobranza = "COBRANZA";
    public const string Pago = "PAGO";
    public const string Planilla = "PLANILLA";
    public const string Produccion = "PRODUCCION";
    public const string Beneficios = "BENEFICIOS";
}

public static class EstadoAsiento
{
    public const string Registrado = "REGISTRADO";
    public const string Anulado = "ANULADO";
}
