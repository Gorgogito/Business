namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.Common;
using Business.Application.DTOs.Ventas;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Ventas;
using Business.Domain.Interfaces;

public class FacturaService : IFacturaService
{
    private readonly IRepository<Factura> _repo;
    private readonly IRepository<Pedido> _pedidoRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICorrelativoService _correlativos;
    private readonly IInventarioService _inventario;
    private readonly IParametroService _parametros;
    private readonly ICuentaPorCobrarService _cuentasPorCobrar;
    private readonly IAsientoContableService _asientos;
    private readonly IConfiguracionContableService _configContable;

    public FacturaService(IRepository<Factura> repo, IRepository<Pedido> pedidoRepo, IUnitOfWork unitOfWork, ICorrelativoService correlativos, IInventarioService inventario, IParametroService parametros, ICuentaPorCobrarService cuentasPorCobrar, IAsientoContableService asientos, IConfiguracionContableService configContable)
    {
        _repo = repo;
        _pedidoRepo = pedidoRepo;
        _unitOfWork = unitOfWork;
        _correlativos = correlativos;
        _inventario = inventario;
        _parametros = parametros;
        _cuentasPorCobrar = cuentasPorCobrar;
        _asientos = asientos;
        _configContable = configContable;
    }

    public async Task<IEnumerable<FacturaDto>> GetAllAsync()
    {
        var items = await _repo.Query().Include(f => f.Cliente).Include(f => f.Detalles).ThenInclude(d => d.Producto).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<FacturaDto?> GetByIdAsync(int id)
    {
        var item = await _repo.Query().Include(f => f.Cliente).Include(f => f.Detalles).ThenInclude(d => d.Producto).FirstOrDefaultAsync(f => f.Id == id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<IEnumerable<FacturaDto>> GetByClienteAsync(int clienteId)
    {
        var items = await _repo.Query().Include(f => f.Cliente).Include(f => f.Detalles).ThenInclude(d => d.Producto).Where(f => f.ClienteId == clienteId).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<FacturaDto> CreateAsync(CreateFacturaDto dto, string userName)
    {
        var esBoleta = string.Equals(dto.TipoDocumento, TipoComprobante.Boleta, StringComparison.OrdinalIgnoreCase);
        var tipoCorrelativo = esBoleta ? TipoComprobante.Boleta : TipoComprobante.Factura;
        var serie = esBoleta ? "B001" : "F001";
        var numero = await _correlativos.SiguienteAsync(tipoCorrelativo, serie);
        var almacenId = dto.AlmacenId > 0 ? dto.AlmacenId : 1;
        var factura = new Factura
        {
            Serie = serie, Numero = numero, Fecha = DateTime.UtcNow,
            ClienteId = dto.ClienteId, PedidoId = dto.PedidoId, AlmacenId = almacenId,
            TipoDocumento = dto.TipoDocumento, Estado = EstadoFactura.Emitida,
            Observaciones = dto.Observaciones, IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = userName
        };

        decimal subTotal = 0;
        decimal costoVentas = 0;
        foreach (var d in dto.Detalles)
        {
            var sub = (d.Cantidad * d.PrecioUnitario) - d.Descuento;
            subTotal += sub;
            factura.Detalles.Add(new FacturaDetalle
            {
                ProductoId = d.ProductoId, Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario,
                Descuento = d.Descuento, SubTotal = sub, IsActive = true, CreatedAt = DateTime.UtcNow
            });

            // Descuenta el stock (salida) validando disponibilidad. Se persiste de forma
            // atómica junto con la factura en el SaveChanges final.
            var mov = await _inventario.RegistrarMovimientoAsync(
                InventarioService.Salida, d.ProductoId, almacenId, d.Cantidad, d.PrecioUnitario,
                $"{serie}-{numero}", $"Venta {dto.TipoDocumento}", userName, validarDisponibilidad: true);
            costoVentas += mov.CostoTotal;
        }
        var igvRate = await _parametros.GetIgvRateAsync();
        factura.SubTotal = subTotal;
        factura.Igv = subTotal * igvRate;
        factura.Total = subTotal + factura.Igv;

        await _repo.AddAsync(factura);

        // Genera la cuenta por cobrar de la factura (atómico con la emisión).
        await _cuentasPorCobrar.GenerarDesdeFacturaAsync(factura, dto.DiasCredito, userName);

        // Asiento contable de venta (cliente / ingreso + IGV; costo / inventario). Las cuentas
        // se resuelven por concepto (configurables por empresa), no hardcodeadas.
        var cuentas = await _configContable.ObtenerMapaAsync();
        await _asientos.GenerarAsientoAsync(
            factura.Fecha, $"Venta {serie}-{numero}", TipoAsiento.Venta, $"{serie}-{numero}",
            new[]
            {
                (cuentas[ConceptosContables.VentaCliente], factura.Total, 0m),
                (cuentas[ConceptosContables.VentaIngreso], 0m, factura.SubTotal),
                (cuentas[ConceptosContables.VentaIgv], 0m, factura.Igv),
                (cuentas[ConceptosContables.VentaCosto], costoVentas, 0m),
                (cuentas[ConceptosContables.VentaInventario], 0m, costoVentas)
            },
            userName);

        await _unitOfWork.SaveChangesAsync();
        return MapToDto(factura);
    }

    public async Task<FacturaDto?> CrearDesdePedidoAsync(int pedidoId, string tipoDocumento, int almacenId, string userName)
    {
        var pedido = await _pedidoRepo.Query().Include(p => p.Detalles).FirstOrDefaultAsync(p => p.Id == pedidoId);
        if (pedido == null) return null;

        if (string.Equals(pedido.Estado, EstadoPedido.Cancelado, StringComparison.OrdinalIgnoreCase))
            throw new BusinessRuleException("No se puede facturar un pedido cancelado.");

        var yaFacturado = await _repo.Query().AnyAsync(f => f.PedidoId == pedidoId && f.IsActive && f.Estado != EstadoFactura.Anulada);
        if (yaFacturado)
            throw new BusinessRuleException("El pedido ya fue facturado.");

        // El pedido queda facturado; el cambio (tracked) se persiste junto con la factura.
        pedido.Estado = EstadoPedido.Facturado;
        pedido.UpdatedAt = DateTime.UtcNow;

        var dto = new CreateFacturaDto
        {
            ClienteId = pedido.ClienteId,
            PedidoId = pedido.Id,
            AlmacenId = almacenId > 0 ? almacenId : 1,
            TipoDocumento = string.IsNullOrWhiteSpace(tipoDocumento) ? TipoComprobante.Factura : tipoDocumento,
            Observaciones = pedido.Observaciones,
            Detalles = pedido.Detalles.Where(d => d.IsActive).Select(d => new CreateFacturaDetalleDto
            {
                ProductoId = d.ProductoId, Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario, Descuento = d.Descuento
            }).ToList()
        };

        return await CreateAsync(dto, userName);
    }

    public async Task<FacturaDto?> UpdateEstadoAsync(int id, string estado)
    {
        var entity = await _repo.Query().Include(f => f.Detalles).FirstOrDefaultAsync(f => f.Id == id);
        if (entity == null) return null;

        var seEstaAnulando = string.Equals(estado, EstadoFactura.Anulada, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(entity.Estado, EstadoFactura.Anulada, StringComparison.OrdinalIgnoreCase);

        if (seEstaAnulando)
        {
            // Anula la cuenta por cobrar (falla si ya tuvo cobros) antes de tocar el stock.
            await _cuentasPorCobrar.AnularPorFacturaAsync(entity.Id);

            // Devuelve al almacén el stock descontado en la emisión (entrada de reversión).
            decimal costoReversion = 0;
            foreach (var d in entity.Detalles.Where(x => x.IsActive))
            {
                var mov = await _inventario.RegistrarMovimientoAsync(
                    InventarioService.Entrada, d.ProductoId, entity.AlmacenId, d.Cantidad, d.PrecioUnitario,
                    $"Anulación {entity.Serie}-{entity.Numero}", "Reversión por anulación", null, validarDisponibilidad: false);
                costoReversion += mov.CostoTotal;
            }

            // Asiento contable de reversión (invierte el asiento de venta).
            var cuentas = await _configContable.ObtenerMapaAsync();
            await _asientos.GenerarAsientoAsync(
                DateTime.UtcNow, $"Anulación {entity.Serie}-{entity.Numero}", TipoAsiento.Venta, $"ANU-{entity.Serie}-{entity.Numero}",
                new[]
                {
                    (cuentas[ConceptosContables.VentaCliente], 0m, entity.Total),
                    (cuentas[ConceptosContables.VentaIngreso], entity.SubTotal, 0m),
                    (cuentas[ConceptosContables.VentaIgv], entity.Igv, 0m),
                    (cuentas[ConceptosContables.VentaCosto], 0m, costoReversion),
                    (cuentas[ConceptosContables.VentaInventario], costoReversion, 0m)
                },
                null);
        }

        entity.Estado = estado; entity.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    private static FacturaDto MapToDto(Factura f) => new()
    {
        Id = f.Id, Serie = f.Serie, Numero = f.Numero, Fecha = f.Fecha,
        ClienteId = f.ClienteId, ClienteNombre = f.Cliente?.RazonSocial, PedidoId = f.PedidoId,
        AlmacenId = f.AlmacenId, Estado = f.Estado, TipoDocumento = f.TipoDocumento,
        SubTotal = f.SubTotal, Igv = f.Igv, Total = f.Total, Observaciones = f.Observaciones,
        Detalles = f.Detalles.Select(d => new FacturaDetalleDto
        {
            Id = d.Id, ProductoId = d.ProductoId, ProductoNombre = d.Producto?.Nombre,
            Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario, Descuento = d.Descuento, SubTotal = d.SubTotal
        }).ToList()
    };
}
