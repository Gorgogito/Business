namespace Business.Domain.Common;

/// <summary>
/// Catálogo de conceptos contables usados por los asientos automáticos (venta, compra,
/// cobranza, pago, planilla, producción). Cada concepto se resuelve a una cuenta del plan
/// contable de la empresa a través de <c>IConfiguracionContableService</c>; si la empresa no
/// configuró el concepto, se usa el código por defecto de <see cref="Defaults"/> (el mismo
/// plan PCGE sembrado de fábrica).
/// </summary>
public static class ConceptosContables
{
    public const string VentaCliente = "VENTA_CLIENTE";
    public const string VentaIngreso = "VENTA_INGRESO";
    public const string VentaIgv = "VENTA_IGV";
    public const string VentaCosto = "VENTA_COSTO";
    public const string VentaInventario = "VENTA_INVENTARIO";
    public const string CompraMercaderia = "COMPRA_MERCADERIA";
    public const string CompraIgv = "COMPRA_IGV";
    public const string CompraProveedor = "COMPRA_PROVEEDOR";
    public const string CompraInventario = "COMPRA_INVENTARIO";
    public const string CompraVariacion = "COMPRA_VARIACION";
    public const string TesoreriaEfectivo = "TESORERIA_EFECTIVO";
    public const string TesoreriaBancos = "TESORERIA_BANCOS";
    public const string PlanillaRemuneraciones = "PLANILLA_REMUNERACIONES";
    public const string PlanillaAportes = "PLANILLA_APORTES";
    public const string PlanillaTributosPorPagar = "PLANILLA_TRIBUTOS_POR_PAGAR";
    public const string PlanillaRemuneracionesPorPagar = "PLANILLA_REMUNERACIONES_POR_PAGAR";
    public const string PlanillaBeneficiosGasto = "PLANILLA_BENEFICIOS_GASTO";
    public const string PlanillaBeneficiosPorPagar = "PLANILLA_BENEFICIOS_POR_PAGAR";
    public const string ProduccionProductosTerminados = "PRODUCCION_PRODUCTOS_TERMINADOS";
    public const string ProduccionVariacion = "PRODUCCION_VARIACION";
    public const string ProduccionManoObraPorPagar = "PRODUCCION_MOD_POR_PAGAR";
    public const string ProduccionIndirectosPorPagar = "PRODUCCION_CIF_POR_PAGAR";

    /// <summary>Concepto, módulo, descripción y código de cuenta PCGE por defecto.</summary>
    public static readonly IReadOnlyList<(string Concepto, string Modulo, string Descripcion, string CuentaDefecto)> Catalogo = new[]
    {
        (VentaCliente, "Ventas", "Cliente / cuenta por cobrar de la factura", "121"),
        (VentaIngreso, "Ventas", "Ingreso por venta de mercadería", "701"),
        (VentaIgv, "Ventas", "IGV de ventas", "4011"),
        (VentaCosto, "Ventas", "Costo de ventas", "691"),
        (VentaInventario, "Ventas", "Salida de mercadería del inventario", "201"),
        (CompraMercaderia, "Compras", "Compra de mercadería", "601"),
        (CompraIgv, "Compras", "IGV de compras", "4011"),
        (CompraProveedor, "Compras", "Proveedor / cuenta por pagar de la recepción", "421"),
        (CompraInventario, "Compras", "Ingreso de mercadería al inventario", "201"),
        (CompraVariacion, "Compras", "Variación de existencias por compra", "611"),
        (TesoreriaEfectivo, "Tesorería", "Caja (cobros/pagos en efectivo)", "101"),
        (TesoreriaBancos, "Tesorería", "Bancos (cobros/pagos no efectivo)", "104"),
        (PlanillaRemuneraciones, "Planillas", "Gasto de remuneraciones", "621"),
        (PlanillaAportes, "Planillas", "Aportes del empleador (EsSalud)", "627"),
        (PlanillaTributosPorPagar, "Planillas", "ONP/AFP/EsSalud por pagar", "403"),
        (PlanillaRemuneracionesPorPagar, "Planillas", "Neto de planilla por pagar", "411"),
        (PlanillaBeneficiosGasto, "Planillas", "Gasto por beneficios sociales (CTS/gratificación/vacaciones)", "629"),
        (PlanillaBeneficiosPorPagar, "Planillas", "Beneficios sociales por pagar", "413"),
        (ProduccionProductosTerminados, "Producción", "Ingreso de productos terminados al inventario", "21"),
        (ProduccionVariacion, "Producción", "Variación de la producción por consumo de materia prima", "71"),
        (ProduccionManoObraPorPagar, "Producción", "Mano de obra directa por pagar", "411"),
        (ProduccionIndirectosPorPagar, "Producción", "Costos indirectos de fabricación por pagar", "421"),
    };

    public static readonly IReadOnlyDictionary<string, string> Defaults =
        Catalogo.ToDictionary(c => c.Concepto, c => c.CuentaDefecto);
}
