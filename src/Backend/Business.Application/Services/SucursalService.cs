namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.DTOs.Configuration;
using Business.Application.Interfaces;
using Business.Domain.Entities.Configuration;
using Business.Domain.Interfaces;

public class SucursalService : ISucursalService
{
    private readonly IRepository<Sucursal> _repo;
    private readonly IUnitOfWork _unitOfWork;

    public SucursalService(IRepository<Sucursal> repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<SucursalDto>> GetAllAsync()
    {
        var items = await _repo.Query().Include(s => s.Empresa).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<SucursalDto?> GetByIdAsync(int id)
    {
        var item = await _repo.Query().Include(s => s.Empresa).FirstOrDefaultAsync(s => s.Id == id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<IEnumerable<SucursalDto>> GetByEmpresaAsync(int empresaId)
    {
        var items = await _repo.Query().Include(s => s.Empresa).Where(s => s.EmpresaId == empresaId).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<SucursalDto> CreateAsync(CreateSucursalDto dto)
    {
        var entity = new Sucursal
        {
            Nombre = dto.Nombre, Codigo = dto.Codigo,
            Direccion = dto.Direccion, Telefono = dto.Telefono,
            EmpresaId = dto.EmpresaId, IsActive = true, CreatedAt = DateTime.UtcNow
        };
        await _repo.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<SucursalDto?> UpdateAsync(int id, CreateSucursalDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return null;
        entity.Nombre = dto.Nombre; entity.Codigo = dto.Codigo;
        entity.Direccion = dto.Direccion; entity.Telefono = dto.Telefono;
        entity.EmpresaId = dto.EmpresaId; entity.UpdatedAt = DateTime.UtcNow;
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

    private static SucursalDto MapToDto(Sucursal s) => new()
    {
        Id = s.Id, Nombre = s.Nombre, Codigo = s.Codigo,
        Direccion = s.Direccion, Telefono = s.Telefono,
        EmpresaId = s.EmpresaId, EmpresaNombre = s.Empresa?.RazonSocial, IsActive = s.IsActive
    };
}
