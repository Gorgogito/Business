namespace Business.Application.Services;

using Business.Application.DTOs.Configuration;
using Business.Application.Interfaces;
using Business.Domain.Entities.Security;
using Business.Domain.Interfaces;

public class EmpresaService : IEmpresaService
{
    private readonly IRepository<Empresa> _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICatalogoEmpresaService _catalogo;

    public EmpresaService(IRepository<Empresa> repo, IUnitOfWork unitOfWork, ICatalogoEmpresaService catalogo)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _catalogo = catalogo;
    }

    public async Task<IEnumerable<EmpresaDto>> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<EmpresaDto?> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<EmpresaDto> CreateAsync(CreateEmpresaDto dto)
    {
        var entity = new Empresa
        {
            RazonSocial = dto.RazonSocial,
            RUC = dto.RUC,
            Direccion = dto.Direccion,
            Telefono = dto.Telefono,
            Email = dto.Email,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        await _repo.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        // Provee a la empresa nueva del plan de cuentas y conceptos de planilla base.
        await _catalogo.ClonarCatalogoBaseAsync(entity.Id);

        return MapToDto(entity);
    }

    public async Task<EmpresaDto?> UpdateAsync(int id, CreateEmpresaDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return null;
        entity.RazonSocial = dto.RazonSocial;
        entity.RUC = dto.RUC;
        entity.Direccion = dto.Direccion;
        entity.Telefono = dto.Telefono;
        entity.Email = dto.Email;
        entity.UpdatedAt = DateTime.UtcNow;
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

    public async Task<bool> AprovisionarCatalogoAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;
        await _catalogo.ClonarCatalogoBaseAsync(id);
        return true;
    }

    private static EmpresaDto MapToDto(Empresa e) => new()
    {
        Id = e.Id, RazonSocial = e.RazonSocial, RUC = e.RUC,
        Direccion = e.Direccion, Telefono = e.Telefono, Email = e.Email,
        LogoUrl = e.LogoUrl, IsActive = e.IsActive
    };
}
