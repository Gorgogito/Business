namespace Business.Application.Services;

using Business.Application.DTOs.Maestros;
using Business.Application.Interfaces;
using Business.Domain.Entities.Maestros;
using Business.Domain.Interfaces;

public class CategoriaService : ICategoriaService
{
    private readonly IRepository<Categoria> _repo;
    private readonly IUnitOfWork _unitOfWork;

    public CategoriaService(IRepository<Categoria> repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CategoriaDto>> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<CategoriaDto?> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<CategoriaDto> CreateAsync(CreateCategoriaDto dto)
    {
        var entity = new Categoria { Nombre = dto.Nombre, Descripcion = dto.Descripcion, IsActive = true, CreatedAt = DateTime.UtcNow };
        await _repo.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<CategoriaDto?> UpdateAsync(int id, CreateCategoriaDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return null;
        entity.Nombre = dto.Nombre; entity.Descripcion = dto.Descripcion; entity.UpdatedAt = DateTime.UtcNow;
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

    private static CategoriaDto MapToDto(Categoria c) => new() { Id = c.Id, Nombre = c.Nombre, Descripcion = c.Descripcion, IsActive = c.IsActive };
}
