namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.DTOs.Inventario;
using Business.Application.Interfaces;
using Business.Domain.Entities.Inventario;
using Business.Domain.Interfaces;

public class MovimientoInventarioService : IMovimientoInventarioService
{
    private readonly IRepository<MovimientoInventario> _repo;
    private readonly IInventarioService _inventario;
    private readonly IUnitOfWork _unitOfWork;

    public MovimientoInventarioService(IRepository<MovimientoInventario> repo, IInventarioService inventario, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _inventario = inventario;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MovimientoInventarioDto>> GetAllAsync()
    {
        var items = await _repo.Query().Include(m => m.Producto).Include(m => m.Almacen).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<MovimientoInventarioDto?> GetByIdAsync(int id)
    {
        var item = await _repo.Query().Include(m => m.Producto).Include(m => m.Almacen).FirstOrDefaultAsync(m => m.Id == id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<IEnumerable<MovimientoInventarioDto>> GetByProductoAsync(int productoId)
    {
        var items = await _repo.Query().Include(m => m.Producto).Include(m => m.Almacen).Where(m => m.ProductoId == productoId).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<MovimientoInventarioDto> CreateAsync(CreateMovimientoDto dto, string userName)
    {
        // Movimiento manual: no valida disponibilidad (permite ajustes/regularizaciones).
        var movimiento = await _inventario.RegistrarMovimientoAsync(
            dto.Tipo, dto.ProductoId, dto.AlmacenId, dto.Cantidad, dto.PrecioUnitario,
            dto.Referencia, dto.Observacion, userName, validarDisponibilidad: false);

        await _unitOfWork.SaveChangesAsync();
        return MapToDto(movimiento);
    }

    private static MovimientoInventarioDto MapToDto(MovimientoInventario m) => new()
    {
        Id = m.Id, Tipo = m.Tipo, ProductoId = m.ProductoId, ProductoNombre = m.Producto?.Nombre,
        AlmacenId = m.AlmacenId, AlmacenNombre = m.Almacen?.Nombre, Cantidad = m.Cantidad,
        PrecioUnitario = m.PrecioUnitario, CostoUnitario = m.CostoUnitario, CostoTotal = m.CostoTotal,
        Referencia = m.Referencia, Observacion = m.Observacion, FechaMovimiento = m.FechaMovimiento
    };
}
