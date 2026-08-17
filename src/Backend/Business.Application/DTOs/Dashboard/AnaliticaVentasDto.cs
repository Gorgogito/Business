namespace Business.Application.DTOs.Dashboard;

/// <summary>
/// Analítica gerencial de ventas para un rango de fechas: totales, margen bruto (venta − costo
/// estándar del producto), ticket promedio y aperturas por período, categoría, producto y cliente.
/// El margen usa el costo estándar (PrecioCompra) del producto; el margen contable real proviene
/// de los estados financieros (701 vs 691).
/// </summary>
public class AnaliticaVentasDto
{
    public DateTime Desde { get; set; }
    public DateTime Hasta { get; set; }

    public decimal TotalVentas { get; set; }
    public decimal TotalCosto { get; set; }
    public decimal MargenBruto { get; set; }
    /// <summary>Margen bruto como % de la venta.</summary>
    public decimal MargenPorcentaje { get; set; }
    public int NumComprobantes { get; set; }
    /// <summary>Venta promedio por comprobante.</summary>
    public decimal TicketPromedio { get; set; }

    public List<VentaPeriodoDto> PorMes { get; set; } = new();
    public List<VentaCategoriaDto> PorCategoria { get; set; } = new();
    public List<ProductoMargenDto> TopProductos { get; set; } = new();
    public List<ClienteVentasDto> TopClientes { get; set; } = new();
}

public class VentaPeriodoDto
{
    public string Periodo { get; set; } = string.Empty; // MM/yyyy
    public decimal Ventas { get; set; }
    public decimal Costo { get; set; }
    public decimal Margen { get; set; }
}

public class VentaCategoriaDto
{
    public string Categoria { get; set; } = string.Empty;
    public decimal Ventas { get; set; }
    public decimal Costo { get; set; }
    public decimal Margen { get; set; }
    public decimal MargenPorcentaje { get; set; }
}

public class ProductoMargenDto
{
    public string Producto { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal Ventas { get; set; }
    public decimal Costo { get; set; }
    public decimal Margen { get; set; }
    public decimal MargenPorcentaje { get; set; }
}

public class ClienteVentasDto
{
    public string Cliente { get; set; } = string.Empty;
    public decimal Ventas { get; set; }
    public int NumComprobantes { get; set; }
}
