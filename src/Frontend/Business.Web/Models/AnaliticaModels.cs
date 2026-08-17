namespace Business.Web.Models;

// ---- Analítica de ventas ----
public class AnaliticaVentasModel
{
    public DateTime Desde { get; set; }
    public DateTime Hasta { get; set; }
    public decimal TotalVentas { get; set; }
    public decimal TotalCosto { get; set; }
    public decimal MargenBruto { get; set; }
    public decimal MargenPorcentaje { get; set; }
    public int NumComprobantes { get; set; }
    public decimal TicketPromedio { get; set; }
    public List<VentaPeriodoModel> PorMes { get; set; } = new();
    public List<VentaCategoriaModel> PorCategoria { get; set; } = new();
    public List<ProductoMargenModel> TopProductos { get; set; } = new();
    public List<ClienteVentasModel> TopClientes { get; set; } = new();
}

public class VentaPeriodoModel
{
    public string Periodo { get; set; } = string.Empty;
    public decimal Ventas { get; set; }
    public decimal Costo { get; set; }
    public decimal Margen { get; set; }
}

public class VentaCategoriaModel
{
    public string Categoria { get; set; } = string.Empty;
    public decimal Ventas { get; set; }
    public decimal Costo { get; set; }
    public decimal Margen { get; set; }
    public decimal MargenPorcentaje { get; set; }
}

public class ProductoMargenModel
{
    public string Producto { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal Ventas { get; set; }
    public decimal Costo { get; set; }
    public decimal Margen { get; set; }
    public decimal MargenPorcentaje { get; set; }
}

public class ClienteVentasModel
{
    public string Cliente { get; set; } = string.Empty;
    public decimal Ventas { get; set; }
    public int NumComprobantes { get; set; }
}

// ---- Analítica de inventario ----
public class AnaliticaInventarioModel
{
    public DateTime Desde { get; set; }
    public DateTime Hasta { get; set; }
    public decimal ValorTotalInventario { get; set; }
    public decimal CostoSalidasPeriodo { get; set; }
    public decimal RotacionGlobal { get; set; }
    public int NumItems { get; set; }
    public int NumStockCritico { get; set; }
    public int NumSinMovimiento { get; set; }
    public List<InventarioItemModel> Items { get; set; } = new();
    public List<InventarioItemModel> StockCritico { get; set; } = new();
    public List<InventarioItemModel> SinMovimiento { get; set; } = new();
}

public class InventarioItemModel
{
    public string Producto { get; set; } = string.Empty;
    public string Almacen { get; set; } = string.Empty;
    public decimal CantidadActual { get; set; }
    public decimal StockMinimo { get; set; }
    public decimal CostoPromedio { get; set; }
    public decimal Valor { get; set; }
    public decimal SalidasCantidad { get; set; }
    public decimal SalidasCosto { get; set; }
    public decimal Rotacion { get; set; }
    public bool BajoMinimo { get; set; }
}

// ---- Analítica financiera ----
public class AnaliticaFinancieraModel
{
    public DateTime Desde { get; set; }
    public DateTime Hasta { get; set; }
    public AgingModel CarteraCobrar { get; set; } = new();
    public AgingModel CarteraPagar { get; set; } = new();
    public EstadoResultadosModel EstadoResultados { get; set; } = new();
}

public class AgingModel
{
    public decimal Total { get; set; }
    public int Documentos { get; set; }
    public List<AgingTramoModel> Tramos { get; set; } = new();
    public List<CarteraItemModel> Detalle { get; set; } = new();
}

public class AgingTramoModel
{
    public string Tramo { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public int Documentos { get; set; }
}

public class CarteraItemModel
{
    public string Contraparte { get; set; } = string.Empty;
    public string Documento { get; set; } = string.Empty;
    public DateTime FechaVencimiento { get; set; }
    public int DiasVencido { get; set; }
    public decimal Saldo { get; set; }
    public string Tramo { get; set; } = string.Empty;
}

public class EstadoResultadosModel
{
    public decimal Ingresos { get; set; }
    public decimal CostoVentas { get; set; }
    public decimal UtilidadBruta { get; set; }
    public decimal Gastos { get; set; }
    public decimal UtilidadNeta { get; set; }
    public List<EstadoLineaModel> Detalle { get; set; } = new();
}

public class EstadoLineaModel
{
    public string Grupo { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public decimal Monto { get; set; }
}
