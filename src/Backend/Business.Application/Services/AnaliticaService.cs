namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.DTOs.Dashboard;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Finanzas;
using Business.Domain.Entities.Inventario;
using Business.Domain.Entities.Ventas;
using Business.Domain.Interfaces;

/// <summary>
/// Analítica gerencial (BI) sobre la data ya registrada. Read-only; respeta el filtro multiempresa
/// del repositorio. El costo de referencia para el margen es el costo estándar (PrecioCompra) del
/// producto.
/// </summary>
public class AnaliticaService : IAnaliticaService
{
    private readonly IRepository<Factura> _facturaRepo;
    private readonly IRepository<Stock> _stockRepo;
    private readonly IRepository<MovimientoInventario> _movRepo;
    private readonly IRepository<CuentaPorCobrar> _cxcRepo;
    private readonly IRepository<CuentaPorPagar> _cxpRepo;
    private readonly IEstadoFinancieroService _estadoFinanciero;

    public AnaliticaService(
        IRepository<Factura> facturaRepo,
        IRepository<Stock> stockRepo,
        IRepository<MovimientoInventario> movRepo,
        IRepository<CuentaPorCobrar> cxcRepo,
        IRepository<CuentaPorPagar> cxpRepo,
        IEstadoFinancieroService estadoFinanciero)
    {
        _facturaRepo = facturaRepo;
        _stockRepo = stockRepo;
        _movRepo = movRepo;
        _cxcRepo = cxcRepo;
        _cxpRepo = cxpRepo;
        _estadoFinanciero = estadoFinanciero;
    }

    public async Task<AnaliticaVentasDto> GetVentasAsync(DateTime desde, DateTime hasta)
    {
        var inicio = desde.Date;
        var fin = hasta.Date.AddDays(1); // exclusivo: incluye todo el día "hasta"

        var facturas = await _facturaRepo.Query()
            .Include(f => f.Cliente)
            .Include(f => f.Detalles).ThenInclude(d => d.Producto).ThenInclude(p => p!.Categoria)
            .Where(f => f.Fecha >= inicio && f.Fecha < fin && f.Estado != EstadoFactura.Anulada)
            .ToListAsync();

        // Líneas aplanadas con venta (base sin IGV) y costo estándar por línea.
        var lineas = facturas.SelectMany(f => f.Detalles.Select(d => new
        {
            f.Fecha,
            Cliente = f.Cliente != null ? f.Cliente.RazonSocial : "(sin cliente)",
            Producto = d.Producto != null ? d.Producto.Nombre : "(sin producto)",
            Categoria = d.Producto?.Categoria != null ? d.Producto.Categoria.Nombre : "(sin categoría)",
            d.Cantidad,
            Venta = d.SubTotal,
            Costo = Math.Round((d.Producto != null ? d.Producto.PrecioCompra : 0m) * d.Cantidad, 2)
        })).ToList();

        var totalVentas = Redondear(lineas.Sum(l => l.Venta));
        var totalCosto = Redondear(lineas.Sum(l => l.Costo));
        var margen = totalVentas - totalCosto;
        var numComprobantes = facturas.Count;

        var dto = new AnaliticaVentasDto
        {
            Desde = inicio,
            Hasta = hasta.Date,
            TotalVentas = totalVentas,
            TotalCosto = totalCosto,
            MargenBruto = margen,
            MargenPorcentaje = Porcentaje(margen, totalVentas),
            NumComprobantes = numComprobantes,
            TicketPromedio = numComprobantes > 0 ? Redondear(totalVentas / numComprobantes) : 0m,

            PorMes = lineas
                .GroupBy(l => new { l.Fecha.Year, l.Fecha.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new VentaPeriodoDto
                {
                    Periodo = $"{g.Key.Month:00}/{g.Key.Year}",
                    Ventas = Redondear(g.Sum(x => x.Venta)),
                    Costo = Redondear(g.Sum(x => x.Costo)),
                    Margen = Redondear(g.Sum(x => x.Venta) - g.Sum(x => x.Costo))
                }).ToList(),

            PorCategoria = lineas
                .GroupBy(l => l.Categoria)
                .Select(g =>
                {
                    var v = Redondear(g.Sum(x => x.Venta));
                    var c = Redondear(g.Sum(x => x.Costo));
                    return new VentaCategoriaDto
                    {
                        Categoria = g.Key, Ventas = v, Costo = c, Margen = v - c,
                        MargenPorcentaje = Porcentaje(v - c, v)
                    };
                })
                .OrderByDescending(x => x.Ventas).ToList(),

            TopProductos = lineas
                .GroupBy(l => l.Producto)
                .Select(g =>
                {
                    var v = Redondear(g.Sum(x => x.Venta));
                    var c = Redondear(g.Sum(x => x.Costo));
                    return new ProductoMargenDto
                    {
                        Producto = g.Key, Cantidad = g.Sum(x => x.Cantidad),
                        Ventas = v, Costo = c, Margen = v - c, MargenPorcentaje = Porcentaje(v - c, v)
                    };
                })
                .OrderByDescending(x => x.Ventas).Take(10).ToList(),

            TopClientes = lineas
                .GroupBy(l => l.Cliente)
                .Select(g => new ClienteVentasDto
                {
                    Cliente = g.Key,
                    Ventas = Redondear(g.Sum(x => x.Venta)),
                    NumComprobantes = g.Select(x => x.Fecha).Count() // aproximación por líneas; ver nota
                })
                .OrderByDescending(x => x.Ventas).Take(10).ToList()
        };

        // Nº de comprobantes por cliente exacto (no por líneas).
        var compPorCliente = facturas
            .GroupBy(f => f.Cliente != null ? f.Cliente.RazonSocial : "(sin cliente)")
            .ToDictionary(g => g.Key, g => g.Count());
        foreach (var c in dto.TopClientes)
            if (compPorCliente.TryGetValue(c.Cliente, out var n)) c.NumComprobantes = n;

        return dto;
    }

    public async Task<AnaliticaInventarioDto> GetInventarioAsync(DateTime desde, DateTime hasta)
    {
        var inicio = desde.Date;
        var fin = hasta.Date.AddDays(1); // exclusivo

        var stocks = await _stockRepo.Query()
            .Include(s => s.Producto)
            .Include(s => s.Almacen)
            .ToListAsync();

        var movimientos = await _movRepo.Query()
            .Where(m => m.FechaMovimiento >= inicio && m.FechaMovimiento < fin)
            .Select(m => new { m.ProductoId, m.AlmacenId, m.Tipo, m.Cantidad, m.CostoTotal })
            .ToListAsync();

        // Salidas del período agrupadas por producto/almacén.
        var salidas = movimientos
            .Where(m => m.Tipo == InventarioService.Salida)
            .GroupBy(m => (m.ProductoId, m.AlmacenId))
            .ToDictionary(g => g.Key, g => (Cantidad: g.Sum(x => x.Cantidad), Costo: g.Sum(x => x.CostoTotal)));

        // Pares producto/almacén con cualquier movimiento en el período (para detectar sin movimiento).
        var conMovimiento = movimientos.Select(m => (m.ProductoId, m.AlmacenId)).ToHashSet();

        var items = stocks.Select(s =>
        {
            var valor = Redondear(s.CantidadActual * s.CostoPromedio);
            salidas.TryGetValue((s.ProductoId, s.AlmacenId), out var sal);
            var salidaCosto = Redondear(sal.Costo);
            return new
            {
                s.ProductoId, s.AlmacenId,
                Dto = new InventarioItemDto
                {
                    Producto = s.Producto?.Nombre ?? "(sin producto)",
                    Almacen = s.Almacen?.Nombre ?? "(sin almacén)",
                    CantidadActual = s.CantidadActual,
                    StockMinimo = s.StockMinimo,
                    CostoPromedio = s.CostoPromedio,
                    Valor = valor,
                    SalidasCantidad = sal.Cantidad,
                    SalidasCosto = salidaCosto,
                    Rotacion = valor > 0 ? Redondear(salidaCosto / valor) : 0m,
                    BajoMinimo = s.StockMinimo > 0 && s.CantidadActual <= s.StockMinimo
                }
            };
        }).ToList();

        var valorTotal = Redondear(items.Sum(i => i.Dto.Valor));
        var costoSalidas = Redondear(items.Sum(i => i.Dto.SalidasCosto));

        return new AnaliticaInventarioDto
        {
            Desde = inicio,
            Hasta = hasta.Date,
            ValorTotalInventario = valorTotal,
            CostoSalidasPeriodo = costoSalidas,
            RotacionGlobal = valorTotal > 0 ? Redondear(costoSalidas / valorTotal) : 0m,
            NumItems = items.Count,
            NumStockCritico = items.Count(i => i.Dto.BajoMinimo),
            NumSinMovimiento = items.Count(i => !conMovimiento.Contains((i.ProductoId, i.AlmacenId))),
            Items = items.OrderByDescending(i => i.Dto.Valor).Select(i => i.Dto).ToList(),
            StockCritico = items.Where(i => i.Dto.BajoMinimo).OrderBy(i => i.Dto.CantidadActual).Select(i => i.Dto).ToList(),
            SinMovimiento = items.Where(i => !conMovimiento.Contains((i.ProductoId, i.AlmacenId)))
                .OrderByDescending(i => i.Dto.Valor).Select(i => i.Dto).ToList()
        };
    }

    public async Task<AnaliticaFinancieraDto> GetFinancieraAsync(DateTime desde, DateTime hasta)
    {
        var corte = hasta.Date; // el aging es a una fecha de corte

        var cxc = await _cxcRepo.Query()
            .Include(c => c.Cliente).Include(c => c.Factura)
            .Where(c => c.SaldoPendiente > 0 && c.Estado != EstadoCuentaPorCobrar.Anulada)
            .ToListAsync();
        var agingCobrar = CalcularAging(cxc.Select(c => (
            c.Cliente != null ? c.Cliente.RazonSocial : "(sin cliente)",
            c.Factura != null ? $"{c.Factura.Serie}-{c.Factura.Numero}" : $"CxC #{c.Id}",
            c.FechaVencimiento, c.SaldoPendiente)), corte);

        var cxp = await _cxpRepo.Query()
            .Include(c => c.Proveedor).Include(c => c.RecepcionCompra)
            .Where(c => c.SaldoPendiente > 0 && c.Estado != EstadoCuentaPorPagar.Anulada)
            .ToListAsync();
        var agingPagar = CalcularAging(cxp.Select(c => (
            c.Proveedor != null ? c.Proveedor.RazonSocial : "(sin proveedor)",
            c.RecepcionCompra != null ? c.RecepcionCompra.Numero : $"CxP #{c.Id}",
            c.FechaVencimiento, c.SaldoPendiente)), corte);

        var estadoResultados = await _estadoFinanciero.EstadoResultadosAsync(desde, hasta);

        return new AnaliticaFinancieraDto
        {
            Desde = desde.Date,
            Hasta = corte,
            CarteraCobrar = agingCobrar,
            CarteraPagar = agingPagar,
            EstadoResultados = estadoResultados
        };
    }

    /// <summary>Arma el aging de una cartera a una fecha de corte, agrupando por tramos de días vencidos.</summary>
    private static AgingDto CalcularAging(
        IEnumerable<(string contraparte, string documento, DateTime venc, decimal saldo)> fuente, DateTime corte)
    {
        var items = fuente.Select(x =>
        {
            var dias = (corte - x.venc.Date).Days;
            return new CarteraItemDto
            {
                Contraparte = x.contraparte, Documento = x.documento, FechaVencimiento = x.venc,
                DiasVencido = dias > 0 ? dias : 0, Saldo = Redondear(x.saldo), Tramo = TramoAging(dias)
            };
        }).ToList();

        var orden = new[] { "Vigente", "1-30", "31-60", "61-90", ">90" };
        var tramos = orden.Select(t => new AgingTramoDto
        {
            Tramo = t,
            Monto = Redondear(items.Where(i => i.Tramo == t).Sum(i => i.Saldo)),
            Documentos = items.Count(i => i.Tramo == t)
        }).ToList();

        return new AgingDto
        {
            Total = Redondear(items.Sum(i => i.Saldo)),
            Documentos = items.Count,
            Tramos = tramos,
            Detalle = items.OrderByDescending(i => i.DiasVencido).ThenByDescending(i => i.Saldo).ToList()
        };
    }

    private static string TramoAging(int dias)
        => dias <= 0 ? "Vigente" : dias <= 30 ? "1-30" : dias <= 60 ? "31-60" : dias <= 90 ? "61-90" : ">90";

    private static decimal Redondear(decimal v) => Math.Round(v, 2, MidpointRounding.AwayFromZero);
    private static decimal Porcentaje(decimal parte, decimal total)
        => total > 0 ? Math.Round(parte / total * 100m, 2, MidpointRounding.AwayFromZero) : 0m;
}
