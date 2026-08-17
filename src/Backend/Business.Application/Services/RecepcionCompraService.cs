namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.DTOs.Compras;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Compras;
using Business.Domain.Interfaces;

public class RecepcionCompraService : IRecepcionCompraService
{
    private readonly IRepository<RecepcionCompra> _repo;
    private readonly IRepository<OrdenCompra> _ordenRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICorrelativoService _correlativos;
    private readonly IInventarioService _inventario;
    private readonly IParametroService _parametros;
    private readonly ICuentaPorPagarService _cuentasPorPagar;
    private readonly IAsientoContableService _asientos;
    private readonly IConfiguracionContableService _configContable;

    public RecepcionCompraService(
        IRepository<RecepcionCompra> repo,
        IRepository<OrdenCompra> ordenRepo,
        IUnitOfWork unitOfWork,
        ICorrelativoService correlativos,
        IInventarioService inventario,
        IParametroService parametros,
        ICuentaPorPagarService cuentasPorPagar,
        IAsientoContableService asientos,
        IConfiguracionContableService configContable)
    {
        _repo = repo;
        _ordenRepo = ordenRepo;
        _unitOfWork = unitOfWork;
        _correlativos = correlativos;
        _inventario = inventario;
        _parametros = parametros;
        _cuentasPorPagar = cuentasPorPagar;
        _asientos = asientos;
        _configContable = configContable;
    }

    public async Task<IEnumerable<RecepcionCompraDto>> GetAllAsync()
    {
        var items = await _repo.Query().Include(r => r.OrdenCompra).Include(r => r.Detalles).ThenInclude(d => d.Producto).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<RecepcionCompraDto?> GetByIdAsync(int id)
    {
        var item = await _repo.Query().Include(r => r.OrdenCompra).Include(r => r.Detalles).ThenInclude(d => d.Producto).FirstOrDefaultAsync(r => r.Id == id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<RecepcionCompraDto> CreateAsync(CreateRecepcionCompraDto dto, string userName)
    {
        var numero = await _correlativos.SiguienteAsync("RECEPCION", "REC");
        var almacenId = dto.AlmacenId > 0 ? dto.AlmacenId : 1;
        var recepcion = new RecepcionCompra
        {
            Numero = numero, Fecha = DateTime.UtcNow, OrdenCompraId = dto.OrdenCompraId, AlmacenId = almacenId,
            Observaciones = dto.Observaciones,
            IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = userName
        };

        var completa = true;
        decimal subTotalRecibido = 0;
        foreach (var d in dto.Detalles)
        {
            recepcion.Detalles.Add(new RecepcionCompraDetalle
            {
                ProductoId = d.ProductoId, CantidadEsperada = d.CantidadEsperada,
                CantidadRecibida = d.CantidadRecibida, PrecioUnitario = d.PrecioUnitario,
                IsActive = true, CreatedAt = DateTime.UtcNow
            });

            if (d.CantidadRecibida < d.CantidadEsperada) completa = false;
            subTotalRecibido += d.CantidadRecibida * d.PrecioUnitario;

            // Ingresa al stock lo efectivamente recibido (entrada), atómico con la recepción.
            if (d.CantidadRecibida > 0)
            {
                await _inventario.RegistrarMovimientoAsync(
                    InventarioService.Entrada, d.ProductoId, almacenId, d.CantidadRecibida, d.PrecioUnitario,
                    $"Recepción {numero}", $"Ingreso por OC {dto.OrdenCompraId}", userName, validarDisponibilidad: false);
            }
        }
        recepcion.Estado = completa ? EstadoRecepcion.Completa : EstadoRecepcion.Parcial;

        await _repo.AddAsync(recepcion);

        // Actualiza la orden de compra: acumula lo recibido por producto y recalcula su estado.
        var orden = await _ordenRepo.Query().Include(o => o.Detalles).FirstOrDefaultAsync(o => o.Id == dto.OrdenCompraId);
        if (orden != null)
        {
            foreach (var d in dto.Detalles)
            {
                var detalleOc = orden.Detalles.FirstOrDefault(x => x.ProductoId == d.ProductoId);
                if (detalleOc != null) detalleOc.CantidadRecibida += d.CantidadRecibida;
            }
            orden.Estado = orden.Detalles.All(x => x.CantidadRecibida >= x.Cantidad) ? EstadoOrdenCompra.Recibida : EstadoOrdenCompra.Parcial;
            orden.UpdatedAt = DateTime.UtcNow;

            // Genera la cuenta por pagar al proveedor por el valor recibido (atómico con la recepción).
            if (subTotalRecibido > 0)
            {
                var igvRate = await _parametros.GetIgvRateAsync();
                var igv = subTotalRecibido * igvRate;
                var montoTotal = subTotalRecibido + igv;
                await _cuentasPorPagar.GenerarDesdeRecepcionAsync(recepcion, orden.ProveedorId, montoTotal, dto.DiasCredito, userName);

                // Asiento contable de compra (compra + IGV / proveedor; mercadería / variación).
                var cuentas = await _configContable.ObtenerMapaAsync();
                await _asientos.GenerarAsientoAsync(
                    recepcion.Fecha, $"Compra recepción {numero}", TipoAsiento.Compra, numero,
                    new[]
                    {
                        (cuentas[ConceptosContables.CompraMercaderia], subTotalRecibido, 0m),
                        (cuentas[ConceptosContables.CompraIgv], igv, 0m),
                        (cuentas[ConceptosContables.CompraProveedor], 0m, montoTotal),
                        (cuentas[ConceptosContables.CompraInventario], subTotalRecibido, 0m),
                        (cuentas[ConceptosContables.CompraVariacion], 0m, subTotalRecibido)
                    },
                    userName);
            }
        }

        await _unitOfWork.SaveChangesAsync();
        return MapToDto(recepcion);
    }

    private static RecepcionCompraDto MapToDto(RecepcionCompra r) => new()
    {
        Id = r.Id, Numero = r.Numero, Fecha = r.Fecha, OrdenCompraId = r.OrdenCompraId,
        OrdenCompraNumero = r.OrdenCompra?.Numero, AlmacenId = r.AlmacenId, Estado = r.Estado, Observaciones = r.Observaciones,
        Detalles = r.Detalles.Select(d => new RecepcionCompraDetalleDto
        {
            Id = d.Id, ProductoId = d.ProductoId, ProductoNombre = d.Producto?.Nombre,
            CantidadEsperada = d.CantidadEsperada, CantidadRecibida = d.CantidadRecibida, PrecioUnitario = d.PrecioUnitario
        }).ToList()
    };
}
