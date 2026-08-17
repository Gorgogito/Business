namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.DTOs.Dashboard;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Maestros;
using Business.Domain.Entities.Ventas;
using Business.Domain.Entities.Compras;
using Business.Domain.Interfaces;

public class DashboardService : IDashboardService
{
    private readonly IRepository<Cliente> _clienteRepo;
    private readonly IRepository<Proveedor> _proveedorRepo;
    private readonly IRepository<Producto> _productoRepo;
    private readonly IRepository<Pedido> _pedidoRepo;
    private readonly IRepository<Factura> _facturaRepo;
    private readonly IRepository<OrdenCompra> _ordenCompraRepo;

    public DashboardService(
        IRepository<Cliente> clienteRepo,
        IRepository<Proveedor> proveedorRepo,
        IRepository<Producto> productoRepo,
        IRepository<Pedido> pedidoRepo,
        IRepository<Factura> facturaRepo,
        IRepository<OrdenCompra> ordenCompraRepo)
    {
        _clienteRepo = clienteRepo;
        _proveedorRepo = proveedorRepo;
        _productoRepo = productoRepo;
        _pedidoRepo = pedidoRepo;
        _facturaRepo = facturaRepo;
        _ordenCompraRepo = ordenCompraRepo;
    }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        var now = DateTime.UtcNow;
        var firstOfMonth = new DateTime(now.Year, now.Month, 1);

        var totalClientes = await _clienteRepo.Query().CountAsync(c => c.IsActive);
        var totalProveedores = await _proveedorRepo.Query().CountAsync(p => p.IsActive);
        var totalProductos = await _productoRepo.Query().CountAsync(p => p.IsActive);
        var pedidosPendientes = await _pedidoRepo.Query().CountAsync(p => p.Estado == EstadoPedido.Pendiente);
        var ocPendientes = await _ordenCompraRepo.Query().CountAsync(o => o.Estado == EstadoOrdenCompra.Pendiente);
        var ventasMes = await _facturaRepo.Query().Where(f => f.Fecha >= firstOfMonth && f.Estado != EstadoFactura.Anulada).SumAsync(f => f.Total);
        var comprasMes = await _ordenCompraRepo.Query().Where(o => o.Fecha >= firstOfMonth && o.Estado != EstadoOrdenCompra.Cancelada).SumAsync(o => o.Total);

        var ventasMensuales = await _facturaRepo.Query()
            .Where(f => f.Fecha >= now.AddMonths(-6) && f.Estado != EstadoFactura.Anulada)
            .GroupBy(f => new { f.Fecha.Year, f.Fecha.Month })
            .Select(g => new VentaMensualDto { Mes = $"{g.Key.Month}/{g.Key.Year}", Total = g.Sum(f => f.Total) })
            .ToListAsync();

        // Productos más vendidos (por monto) en los últimos 6 meses.
        var facturasConDetalle = await _facturaRepo.Query()
            .Include(f => f.Detalles).ThenInclude(d => d.Producto)
            .Where(f => f.Fecha >= now.AddMonths(-6) && f.Estado != EstadoFactura.Anulada)
            .ToListAsync();
        var topProductos = facturasConDetalle
            .SelectMany(f => f.Detalles)
            .GroupBy(d => d.Producto != null ? d.Producto.Nombre : "(sin producto)")
            .Select(g => new ProductoTopDto { Nombre = g.Key, TotalVendido = g.Sum(x => x.SubTotal) })
            .OrderByDescending(x => x.TotalVendido)
            .Take(5)
            .ToList();

        return new DashboardDto
        {
            TotalClientes = totalClientes,
            TotalProveedores = totalProveedores,
            TotalProductos = totalProductos,
            VentasMes = ventasMes,
            ComprasMes = comprasMes,
            PedidosPendientes = pedidosPendientes,
            OrdenesCompraPendientes = ocPendientes,
            VentasMensuales = ventasMensuales,
            TopProductos = topProductos
        };
    }
}
