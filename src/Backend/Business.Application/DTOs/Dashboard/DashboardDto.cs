namespace Business.Application.DTOs.Dashboard;

public class DashboardDto
{
    public int TotalClientes { get; set; }
    public int TotalProveedores { get; set; }
    public int TotalProductos { get; set; }
    public decimal VentasMes { get; set; }
    public decimal ComprasMes { get; set; }
    public int PedidosPendientes { get; set; }
    public int OrdenesCompraPendientes { get; set; }
    public List<VentaMensualDto> VentasMensuales { get; set; } = new();
    public List<ProductoTopDto> TopProductos { get; set; } = new();
}

public class VentaMensualDto
{
    public string Mes { get; set; } = string.Empty;
    public decimal Total { get; set; }
}

public class ProductoTopDto
{
    public string Nombre { get; set; } = string.Empty;
    public decimal TotalVendido { get; set; }
}
