namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.Common;
using Business.Application.DTOs.Produccion;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Produccion;
using Business.Domain.Interfaces;

public class OrdenFabricacionService : IOrdenFabricacionService
{
    private readonly IRepository<OrdenFabricacion> _repo;
    private readonly IRepository<Receta> _recetaRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICorrelativoService _correlativos;
    private readonly IInventarioService _inventario;
    private readonly IAsientoContableService _asientos;
    private readonly IConfiguracionContableService _configContable;

    public OrdenFabricacionService(
        IRepository<OrdenFabricacion> repo,
        IRepository<Receta> recetaRepo,
        IUnitOfWork unitOfWork,
        ICorrelativoService correlativos,
        IInventarioService inventario,
        IAsientoContableService asientos,
        IConfiguracionContableService configContable)
    {
        _repo = repo;
        _recetaRepo = recetaRepo;
        _unitOfWork = unitOfWork;
        _correlativos = correlativos;
        _inventario = inventario;
        _asientos = asientos;
        _configContable = configContable;
    }

    public async Task<IEnumerable<OrdenFabricacionDto>> GetAllAsync()
    {
        var items = await CargarQuery().OrderByDescending(o => o.Fecha).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<OrdenFabricacionDto?> GetByIdAsync(int id)
    {
        var item = await CargarQuery().FirstOrDefaultAsync(o => o.Id == id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<OrdenFabricacionDto> CreateAsync(CreateOrdenFabricacionDto dto, string userName)
    {
        if (dto.CantidadProducir <= 0)
            throw new BusinessRuleException("La cantidad a producir debe ser mayor a cero.");

        var receta = await _recetaRepo.Query().Include(r => r.Detalles)
            .FirstOrDefaultAsync(r => r.ProductoId == dto.ProductoId && r.Estado == EstadoReceta.Activa);
        if (receta == null)
            throw new BusinessRuleException("El producto no tiene una receta activa para fabricar.");

        var factor = dto.CantidadProducir / receta.CantidadProducida;
        var numero = await _correlativos.SiguienteAsync("ORDEN_FAB", "OF");
        var orden = new OrdenFabricacion
        {
            Numero = numero, ProductoId = dto.ProductoId, RecetaId = receta.Id,
            CantidadProducir = dto.CantidadProducir, AlmacenId = dto.AlmacenId > 0 ? dto.AlmacenId : 1,
            Fecha = DateTime.UtcNow, Estado = EstadoOrdenFabricacion.Pendiente,
            IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = userName
        };
        foreach (var ins in receta.Detalles.Where(d => d.IsActive))
        {
            orden.Detalles.Add(new OrdenFabricacionDetalle
            {
                InsumoId = ins.InsumoId, Cantidad = ins.Cantidad * factor,
                IsActive = true, CreatedAt = DateTime.UtcNow
            });
        }

        await _repo.AddAsync(orden);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(orden);
    }

    public async Task<OrdenFabricacionDto?> ProcesarAsync(int id, ProcesarOrdenFabricacionDto dto, string userName)
    {
        var orden = await CargarQuery().FirstOrDefaultAsync(o => o.Id == id);
        if (orden == null) return null;
        if (orden.Estado != EstadoOrdenFabricacion.Pendiente)
            throw new BusinessRuleException("Solo se pueden procesar órdenes de fabricación pendientes.");
        if (dto.CostoManoObra < 0 || dto.CostoIndirecto < 0)
            throw new BusinessRuleException("Los costos de mano de obra y CIF no pueden ser negativos.");

        // 1) Consumir la materia prima (salida valorizada al costo promedio).
        decimal costoMp = 0;
        foreach (var det in orden.Detalles.Where(d => d.IsActive))
        {
            var mov = await _inventario.RegistrarMovimientoAsync(
                InventarioService.Salida, det.InsumoId, orden.AlmacenId, det.Cantidad, 0m,
                $"OF {orden.Numero}", "Consumo de materia prima", userName, validarDisponibilidad: true);
            det.CostoUnitario = mov.CostoUnitario;
            det.CostoTotal = mov.CostoTotal;
            costoMp += mov.CostoTotal;
        }

        // 2) Costeo: MP + MOD + CIF.
        orden.CostoMateriaPrima = costoMp;
        orden.CostoManoObra = dto.CostoManoObra;
        orden.CostoIndirecto = dto.CostoIndirecto;
        orden.CostoTotal = costoMp + dto.CostoManoObra + dto.CostoIndirecto;
        orden.CostoUnitario = Math.Round(orden.CostoTotal / orden.CantidadProducir, 4, MidpointRounding.AwayFromZero);

        // 3) Ingresar el producto terminado al inventario, valorizado al costo unitario.
        await _inventario.RegistrarMovimientoAsync(
            InventarioService.Entrada, orden.ProductoId, orden.AlmacenId, orden.CantidadProducir, orden.CostoUnitario,
            $"OF {orden.Numero}", "Ingreso de producción terminada", userName, validarDisponibilidad: false);

        orden.Estado = EstadoOrdenFabricacion.Terminada;
        orden.UpdatedAt = DateTime.UtcNow; orden.UpdatedBy = userName;

        // 4) Asiento contable: el producto terminado se activa por su costo total; la MP
        //    transformada se acredita a variación de producción, la MOD queda por pagar a
        //    planilla y el CIF como cuenta por pagar de servicios.
        var cuentas = await _configContable.ObtenerMapaAsync();
        await _asientos.GenerarAsientoAsync(
            orden.Fecha, $"Producción {orden.Numero}", TipoAsiento.Produccion, orden.Numero,
            new[]
            {
                (cuentas[ConceptosContables.ProduccionProductosTerminados], orden.CostoTotal, 0m),
                (cuentas[ConceptosContables.ProduccionVariacion], 0m, orden.CostoMateriaPrima),
                (cuentas[ConceptosContables.ProduccionManoObraPorPagar], 0m, orden.CostoManoObra),
                (cuentas[ConceptosContables.ProduccionIndirectosPorPagar], 0m, orden.CostoIndirecto)
            },
            userName);

        await _unitOfWork.SaveChangesAsync();
        return MapToDto(orden);
    }

    public async Task<OrdenFabricacionDto?> AnularAsync(int id, string userName)
    {
        var orden = await CargarQuery().FirstOrDefaultAsync(o => o.Id == id);
        if (orden == null) return null;
        if (orden.Estado == EstadoOrdenFabricacion.Anulada)
            throw new BusinessRuleException("La orden ya está anulada.");

        if (orden.Estado == EstadoOrdenFabricacion.Terminada)
        {
            // Saca del stock el producto terminado producido (valida que no se haya consumido).
            await _inventario.RegistrarMovimientoAsync(
                InventarioService.Salida, orden.ProductoId, orden.AlmacenId, orden.CantidadProducir, orden.CostoUnitario,
                $"Anulación OF {orden.Numero}", "Reversión de producción", userName, validarDisponibilidad: true);

            // Reingresa la materia prima consumida a su costo de salida.
            foreach (var det in orden.Detalles.Where(d => d.IsActive))
            {
                await _inventario.RegistrarMovimientoAsync(
                    InventarioService.Entrada, det.InsumoId, orden.AlmacenId, det.Cantidad, det.CostoUnitario,
                    $"Anulación OF {orden.Numero}", "Reversión consumo MP", userName, validarDisponibilidad: false);
            }

            // Asiento inverso al de producción.
            var cuentas = await _configContable.ObtenerMapaAsync();
            await _asientos.GenerarAsientoAsync(
                orden.Fecha, $"Anulación producción {orden.Numero}", TipoAsiento.Produccion, $"ANU-{orden.Numero}",
                new[]
                {
                    (cuentas[ConceptosContables.ProduccionProductosTerminados], 0m, orden.CostoTotal),
                    (cuentas[ConceptosContables.ProduccionVariacion], orden.CostoMateriaPrima, 0m),
                    (cuentas[ConceptosContables.ProduccionManoObraPorPagar], orden.CostoManoObra, 0m),
                    (cuentas[ConceptosContables.ProduccionIndirectosPorPagar], orden.CostoIndirecto, 0m)
                },
                userName);
        }

        orden.Estado = EstadoOrdenFabricacion.Anulada;
        orden.UpdatedAt = DateTime.UtcNow; orden.UpdatedBy = userName;
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(orden);
    }

    private IQueryable<OrdenFabricacion> CargarQuery() =>
        _repo.Query().Include(o => o.Producto).Include(o => o.Detalles).ThenInclude(d => d.Insumo);

    private static OrdenFabricacionDto MapToDto(OrdenFabricacion o) => new()
    {
        Id = o.Id, Numero = o.Numero, ProductoId = o.ProductoId, ProductoNombre = o.Producto?.Nombre,
        RecetaId = o.RecetaId, CantidadProducir = o.CantidadProducir, AlmacenId = o.AlmacenId, Fecha = o.Fecha, Estado = o.Estado,
        CostoMateriaPrima = o.CostoMateriaPrima, CostoManoObra = o.CostoManoObra, CostoIndirecto = o.CostoIndirecto,
        CostoTotal = o.CostoTotal, CostoUnitario = o.CostoUnitario,
        Detalles = o.Detalles.Where(d => d.IsActive).Select(d => new OrdenFabricacionDetalleDto
        {
            Id = d.Id, InsumoId = d.InsumoId, InsumoNombre = d.Insumo?.Nombre,
            Cantidad = d.Cantidad, CostoUnitario = d.CostoUnitario, CostoTotal = d.CostoTotal
        }).ToList()
    };
}
