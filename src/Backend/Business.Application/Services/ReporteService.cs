namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.DTOs.Finanzas;
using Business.Application.DTOs.Reportes;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Ventas;
using Business.Domain.Interfaces;

public class ReporteService : IReporteService
{
    private readonly IRepository<Factura> _facturaRepo;
    private readonly IStockService _stockService;
    private readonly ICuentaPorCobrarService _cuentasPorCobrar;
    private readonly ICuentaPorPagarService _cuentasPorPagar;

    public ReporteService(
        IRepository<Factura> facturaRepo,
        IStockService stockService,
        ICuentaPorCobrarService cuentasPorCobrar,
        ICuentaPorPagarService cuentasPorPagar)
    {
        _facturaRepo = facturaRepo;
        _stockService = stockService;
        _cuentasPorCobrar = cuentasPorCobrar;
        _cuentasPorPagar = cuentasPorPagar;
    }

    public async Task<ReporteVentasDto> VentasPorPeriodoAsync(DateTime desde, DateTime hasta)
    {
        // Rango inclusivo por día completo.
        var d = desde.Date;
        var h = hasta.Date.AddDays(1).AddTicks(-1);

        var facturas = await _facturaRepo.Query()
            .Include(f => f.Cliente)
            .Where(f => f.Fecha >= d && f.Fecha <= h && f.Estado != EstadoFactura.Anulada)
            .OrderBy(f => f.Fecha)
            .ToListAsync();

        var detalle = facturas.Select(f => new VentaLineaDto
        {
            Fecha = f.Fecha,
            Comprobante = $"{f.Serie}-{f.Numero}",
            TipoDocumento = f.TipoDocumento,
            Cliente = f.Cliente?.RazonSocial,
            SubTotal = f.SubTotal, Igv = f.Igv, Total = f.Total, Estado = f.Estado
        }).ToList();

        return new ReporteVentasDto
        {
            Desde = d, Hasta = hasta.Date,
            CantidadComprobantes = detalle.Count,
            TotalSubTotal = detalle.Sum(x => x.SubTotal),
            TotalIgv = detalle.Sum(x => x.Igv),
            TotalVentas = detalle.Sum(x => x.Total),
            Detalle = detalle
        };
    }

    public async Task<ReporteInventarioDto> ValorizacionInventarioAsync()
    {
        var stocks = await _stockService.GetAllAsync();
        var items = stocks.Select(s => new InventarioLineaDto
        {
            Codigo = s.ProductoCodigo, Producto = s.ProductoNombre, Almacen = s.AlmacenNombre,
            Cantidad = s.CantidadActual, CostoPromedio = s.CostoPromedio, Valor = s.ValorInventario
        }).ToList();

        return new ReporteInventarioDto
        {
            TotalItems = items.Count,
            TotalValorizado = items.Sum(x => x.Valor),
            Items = items
        };
    }

    public Task<IEnumerable<CuentaPorCobrarDto>> CarteraPorCobrarAsync() => _cuentasPorCobrar.GetPendientesAsync();

    public Task<IEnumerable<CuentaPorPagarDto>> CarteraPorPagarAsync() => _cuentasPorPagar.GetPendientesAsync();
}
