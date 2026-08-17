namespace Business.Application.Services;

using Business.Application.DTOs.Inventario;
using Business.Application.Interfaces;
using Business.Domain.Entities.Inventario;
using Business.Domain.Interfaces;

public class AlmacenService : IAlmacenService
{
    private readonly IRepository<Almacen> _repo;
    private readonly IUnitOfWork _unitOfWork;

    public AlmacenService(IRepository<Almacen> repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AlmacenDto>> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<AlmacenDto?> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<AlmacenDto> CreateAsync(CreateAlmacenDto dto)
    {
        var entity = new Almacen { Codigo = dto.Codigo, Nombre = dto.Nombre, Ubicacion = dto.Ubicacion, IsActive = true, CreatedAt = DateTime.UtcNow };
        await _repo.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<AlmacenDto?> UpdateAsync(int id, CreateAlmacenDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return null;
        entity.Codigo = dto.Codigo; entity.Nombre = dto.Nombre; entity.Ubicacion = dto.Ubicacion; entity.UpdatedAt = DateTime.UtcNow;
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

    private static AlmacenDto MapToDto(Almacen a) => new() { Id = a.Id, Codigo = a.Codigo, Nombre = a.Nombre, Ubicacion = a.Ubicacion, IsActive = a.IsActive };
}
