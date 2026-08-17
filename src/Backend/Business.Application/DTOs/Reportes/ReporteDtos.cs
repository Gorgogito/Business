namespace Business.Application.DTOs.Reportes;

public class ReporteVentasDto
{
    public DateTime Desde { get; set; }
    public DateTime Hasta { get; set; }
    public int CantidadComprobantes { get; set; }
    public decimal TotalSubTotal { get; set; }
    public decimal TotalIgv { get; set; }
    public decimal TotalVentas { get; set; }
    public List<VentaLineaDto> Detalle { get; set; } = new();
}

public class VentaLineaDto
{
    public DateTime Fecha { get; set; }
    public string Comprobante { get; set; } = string.Empty;
    public string TipoDocumento { get; set; } = string.Empty;
    public string? Cliente { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Igv { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = string.Empty;
}

public class ReporteInventarioDto
{
    public int TotalItems { get; set; }
    public decimal TotalValorizado { get; set; }
    public List<InventarioLineaDto> Items { get; set; } = new();
}

public class InventarioLineaDto
{
    public string? Codigo { get; set; }
    public string? Producto { get; set; }
    public string? Almacen { get; set; }
    public decimal Cantidad { get; set; }
    public decimal CostoPromedio { get; set; }
    public decimal Valor { get; set; }
}
