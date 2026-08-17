namespace Business.Application.DTOs.Dashboard;

/// <summary>
/// Analítica gerencial de inventario: valorización a costo promedio, rotación (costo de salidas del
/// período / valor del inventario), stock crítico (bajo mínimo) y productos sin movimiento en el rango.
/// </summary>
public class AnaliticaInventarioDto
{
    public DateTime Desde { get; set; }
    public DateTime Hasta { get; set; }

    public decimal ValorTotalInventario { get; set; }
    public decimal CostoSalidasPeriodo { get; set; }
    /// <summary>Rotación global = costo de salidas del período / valor del inventario.</summary>
    public decimal RotacionGlobal { get; set; }

    public int NumItems { get; set; }
    public int NumStockCritico { get; set; }
    public int NumSinMovimiento { get; set; }

    public List<InventarioItemDto> Items { get; set; } = new();
    public List<InventarioItemDto> StockCritico { get; set; } = new();
    public List<InventarioItemDto> SinMovimiento { get; set; } = new();
}

public class InventarioItemDto
{
    public string Producto { get; set; } = string.Empty;
    public string Almacen { get; set; } = string.Empty;
    public decimal CantidadActual { get; set; }
    public decimal StockMinimo { get; set; }
    public decimal CostoPromedio { get; set; }
    public decimal Valor { get; set; }
    /// <summary>Cantidad y costo de las salidas del producto/almacén en el período.</summary>
    public decimal SalidasCantidad { get; set; }
    public decimal SalidasCosto { get; set; }
    /// <summary>Rotación del ítem = costo de salidas / valor del inventario.</summary>
    public decimal Rotacion { get; set; }
    public bool BajoMinimo { get; set; }
}
