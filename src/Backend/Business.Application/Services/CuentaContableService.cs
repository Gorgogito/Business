namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.Common;
using Business.Application.DTOs.Contabilidad;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Contabilidad;
using Business.Domain.Interfaces;

public class CuentaContableService : ICuentaContableService
{
    private readonly IRepository<CuentaContable> _repo;
    private readonly IUnitOfWork _unitOfWork;

    public CuentaContableService(IRepository<CuentaContable> repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CuentaContableDto>> GetAllAsync()
    {
        var items = await _repo.Query().OrderBy(c => c.Codigo).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<CuentaContableDto?> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<CuentaContableDto?> GetByCodigoAsync(string codigo)
    {
        var item = (await _repo.FindAsync(c => c.Codigo == codigo)).FirstOrDefault();
        return item != null ? MapToDto(item) : null;
    }

    public async Task<CuentaContableDto> CreateAsync(CreateCuentaContableDto dto, string userName)
    {
        if (await _repo.ExistsAsync(c => c.Codigo == dto.Codigo))
            throw new BusinessRuleException($"Ya existe una cuenta con el código {dto.Codigo}.");

        var entity = new CuentaContable
        {
            Codigo = dto.Codigo,
            Nombre = dto.Nombre,
            Clase = dto.Clase,
            Naturaleza = NaturalezaCuenta.DesdeClase(dto.Clase),
            Nivel = dto.Codigo.Length,
            EsMovimiento = dto.EsMovimiento,
            IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = userName
        };
        await _repo.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<CuentaContableDto?> UpdateAsync(int id, CreateCuentaContableDto dto, string userName)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return null;

        entity.Nombre = dto.Nombre;
        entity.Clase = dto.Clase;
        entity.Naturaleza = NaturalezaCuenta.DesdeClase(dto.Clase);
        entity.EsMovimiento = dto.EsMovimiento;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = userName;
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

    private static CuentaContableDto MapToDto(CuentaContable c) => new()
    {
        Id = c.Id, Codigo = c.Codigo, Nombre = c.Nombre, Clase = c.Clase,
        Naturaleza = c.Naturaleza, Nivel = c.Nivel, EsMovimiento = c.EsMovimiento
    };
}
