namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.Common;
using Business.Application.DTOs.Rrhh;
using Business.Application.Interfaces;
using Business.Domain.Entities.Rrhh;
using Business.Domain.Interfaces;

public class TasaAfpService : ITasaAfpService
{
    private readonly IRepository<TasaAfp> _repo;
    private readonly IUnitOfWork _unitOfWork;

    public TasaAfpService(IRepository<TasaAfp> repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TasaAfpDto>> GetAllAsync()
    {
        var items = await _repo.Query().OrderBy(t => t.Nombre).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<TasaAfpDto?> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<TasaAfpDto> CreateAsync(CreateTasaAfpDto dto, string userName)
    {
        if (await _repo.ExistsAsync(t => t.Nombre == dto.Nombre))
            throw new BusinessRuleException($"Ya existe una AFP con el nombre {dto.Nombre}.");
        Validar(dto);

        var entity = new TasaAfp
        {
            Nombre = dto.Nombre, AporteFondo = dto.AporteFondo,
            ComisionFlujo = dto.ComisionFlujo, PrimaSeguro = dto.PrimaSeguro,
            IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = userName
        };
        await _repo.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<TasaAfpDto?> UpdateAsync(int id, CreateTasaAfpDto dto, string userName)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return null;
        Validar(dto);

        entity.Nombre = dto.Nombre;
        entity.AporteFondo = dto.AporteFondo;
        entity.ComisionFlujo = dto.ComisionFlujo;
        entity.PrimaSeguro = dto.PrimaSeguro;
        entity.UpdatedAt = DateTime.UtcNow; entity.UpdatedBy = userName;
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

    private static void Validar(CreateTasaAfpDto dto)
    {
        if (dto.AporteFondo <= 0) throw new BusinessRuleException("El aporte al fondo debe ser mayor a cero.");
        if (dto.ComisionFlujo < 0 || dto.PrimaSeguro < 0)
            throw new BusinessRuleException("Las tasas no pueden ser negativas.");
    }

    private static TasaAfpDto MapToDto(TasaAfp t) => new()
    {
        Id = t.Id, Nombre = t.Nombre, AporteFondo = t.AporteFondo,
        ComisionFlujo = t.ComisionFlujo, PrimaSeguro = t.PrimaSeguro
    };
}
