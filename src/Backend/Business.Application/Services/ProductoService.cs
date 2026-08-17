namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.DTOs.Maestros;
using Business.Application.Interfaces;
using Business.Domain.Entities.Maestros;
using Business.Domain.Interfaces;

public class ProductoService : IProductoService
{
    private readonly IRepository<Producto> _repo;
    private readonly IUnitOfWork _unitOfWork;

    public ProductoService(IRepository<Producto> repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ProductoDto>> GetAllAsync()
    {
        var items = await _repo.Query().Include(p => p.Categoria).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<ProductoDto?> GetByIdAsync(int id)
    {
        var item = await _repo.Query().Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<IEnumerable<ProductoDto>> GetByCategoriaAsync(int categoriaId)
    {
        var items = await _repo.Query().Include(p => p.Categoria).Where(p => p.CategoriaId == categoriaId).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<ProductoDto> CreateAsync(CreateProductoDto dto)
    {
        var entity = new Producto
        {
            Codigo = dto.Codigo, Nombre = dto.Nombre, Descripcion = dto.Descripcion,
            PrecioCompra = dto.PrecioCompra, PrecioVenta = dto.PrecioVenta,
            Unidad = dto.Unidad, CategoriaId = dto.CategoriaId,
            IsActive = true, CreatedAt = DateTime.UtcNow
        };
        await _repo.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<ProductoDto?> UpdateAsync(int id, CreateProductoDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return null;
        entity.Codigo = dto.Codigo; entity.Nombre = dto.Nombre; entity.Descripcion = dto.Descripcion;
        entity.PrecioCompra = dto.PrecioCompra; entity.PrecioVenta = dto.PrecioVenta;
        entity.Unidad = dto.Unidad; entity.CategoriaId = dto.CategoriaId; entity.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;
        entity.IsActive = false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private static ProductoDto MapToDto(Producto p) => new()
    {
        Id = p.Id, Codigo = p.Codigo, Nombre = p.Nombre, Descripcion = p.Descripcion,
        PrecioCompra = p.PrecioCompra, PrecioVenta = p.PrecioVenta,
        Unidad = p.Unidad, CategoriaId = p.CategoriaId,
        CategoriaNombre = p.Categoria?.Nombre, IsActive = p.IsActive
    };
}
