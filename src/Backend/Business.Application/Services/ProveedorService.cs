namespace Business.Application.Services;

using Business.Application.DTOs.Maestros;
using Business.Application.Interfaces;
using Business.Domain.Entities.Maestros;
using Business.Domain.Interfaces;

public class ProveedorService : IProveedorService
{
    private readonly IRepository<Proveedor> _repo;
    private readonly IUnitOfWork _unitOfWork;

    public ProveedorService(IRepository<Proveedor> repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ProveedorDto>> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<ProveedorDto?> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<ProveedorDto> CreateAsync(CreateProveedorDto dto)
    {
        var entity = new Proveedor
        {
            RazonSocial = dto.RazonSocial, RUC = dto.RUC,
            Direccion = dto.Direccion, Telefono = dto.Telefono,
            Email = dto.Email, TipoDocumento = dto.TipoDocumento,
            IsActive = true, CreatedAt = DateTime.UtcNow
        };
        await _repo.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<ProveedorDto?> UpdateAsync(int id, CreateProveedorDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return null;
        entity.RazonSocial = dto.RazonSocial; entity.RUC = dto.RUC;
        entity.Direccion = dto.Direccion; entity.Telefono = dto.Telefono;
        entity.Email = dto.Email; entity.UpdatedAt = DateTime.UtcNow;
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

    private static ProveedorDto MapToDto(Proveedor p) => new()
    {
        Id = p.Id, RazonSocial = p.RazonSocial, RUC = p.RUC,
        Direccion = p.Direccion, Telefono = p.Telefono, Email = p.Email,
        TipoDocumento = p.TipoDocumento, IsActive = p.IsActive
    };
}
