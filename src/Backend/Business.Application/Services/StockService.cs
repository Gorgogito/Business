namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.DTOs.Inventario;
using Business.Application.Interfaces;
using Business.Domain.Entities.Inventario;
using Business.Domain.Interfaces;

public class StockService : IStockService
{
    private readonly IRepository<Stock> _repo;

    public StockService(IRepository<Stock> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<StockDto>> GetAllAsync()
    {
        var items = await _repo.Query().Include(s => s.Producto).Include(s => s.Almacen).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<StockDto>> GetByProductoAsync(int productoId)
    {
        var items = await _repo.Query().Include(s => s.Producto).Include(s => s.Almacen).Where(s => s.ProductoId == productoId).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<StockDto>> GetByAlmacenAsync(int almacenId)
    {
        var items = await _repo.Query().Include(s => s.Producto).Include(s => s.Almacen).Where(s => s.AlmacenId == almacenId).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<StockDto?> GetByProductoAlmacenAsync(int productoId, int almacenId)
    {
        var item = await _repo.Query().Include(s => s.Producto).Include(s => s.Almacen)
            .FirstOrDefaultAsync(s => s.ProductoId == productoId && s.AlmacenId == almacenId);
        return item != null ? MapToDto(item) : null;
    }

    private static StockDto MapToDto(Stock s) => new()
    {
        Id = s.Id, ProductoId = s.ProductoId, ProductoNombre = s.Producto?.Nombre, ProductoCodigo = s.Producto?.Codigo,
        AlmacenId = s.AlmacenId, AlmacenNombre = s.Almacen?.Nombre,
        CantidadActual = s.CantidadActual, StockMinimo = s.StockMinimo, StockMaximo = s.StockMaximo,
        CostoPromedio = s.CostoPromedio, ValorInventario = s.CantidadActual * s.CostoPromedio
    };
}
