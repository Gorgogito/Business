namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.Common;
using Business.Application.DTOs.Rrhh;
using Business.Application.Interfaces;
using Business.Domain.Entities.Rrhh;
using Business.Domain.Interfaces;

public class ConceptoPlanillaService : IConceptoPlanillaService
{
    private readonly IRepository<ConceptoPlanilla> _repo;
    private readonly IUnitOfWork _unitOfWork;

    public ConceptoPlanillaService(IRepository<ConceptoPlanilla> repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ConceptoPlanillaDto>> GetAllAsync()
    {
        var items = await _repo.Query().OrderBy(c => c.Orden).ThenBy(c => c.Codigo).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<ConceptoPlanillaDto?> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<ConceptoPlanillaDto> CreateAsync(CreateConceptoPlanillaDto dto, string userName)
    {
        if (await _repo.ExistsAsync(c => c.Codigo == dto.Codigo))
            throw new BusinessRuleException($"Ya existe un concepto con el código {dto.Codigo}.");
        ValidarCalculo(dto);

        var entity = new ConceptoPlanilla
        {
            Codigo = dto.Codigo, Nombre = dto.Nombre, Tipo = dto.Tipo, MetodoCalculo = dto.MetodoCalculo,
            Porcentaje = dto.Porcentaje, MontoFijo = dto.MontoFijo,
            AfectaAfp = dto.AfectaAfp, AfectaEssalud = dto.AfectaEssalud, EsSistema = false, Orden = dto.Orden,
            IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = userName
        };
        await _repo.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<ConceptoPlanillaDto?> UpdateAsync(int id, CreateConceptoPlanillaDto dto, string userName)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return null;
        ValidarCalculo(dto);

        // Los conceptos del sistema conservan su código, tipo y método (solo se afinan valores).
        if (!entity.EsSistema)
        {
            entity.Codigo = dto.Codigo;
            entity.Tipo = dto.Tipo;
            entity.MetodoCalculo = dto.MetodoCalculo;
        }
        entity.Nombre = dto.Nombre;
        entity.Porcentaje = dto.Porcentaje;
        entity.MontoFijo = dto.MontoFijo;
        entity.AfectaAfp = dto.AfectaAfp;
        entity.AfectaEssalud = dto.AfectaEssalud;
        entity.Orden = dto.Orden;
        entity.UpdatedAt = DateTime.UtcNow; entity.UpdatedBy = userName;
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;
        if (entity.EsSistema)
            throw new BusinessRuleException("No se puede eliminar un concepto base del sistema.");
        entity.IsActive = false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private static void ValidarCalculo(CreateConceptoPlanillaDto dto)
    {
        if (dto.MetodoCalculo == Business.Domain.Common.MetodoCalculo.Porcentual && (dto.Porcentaje is null or <= 0))
            throw new BusinessRuleException("Un concepto porcentual requiere un porcentaje mayor a cero.");
        if (dto.MetodoCalculo == Business.Domain.Common.MetodoCalculo.Fijo && (dto.MontoFijo is null or <= 0))
            throw new BusinessRuleException("Un concepto fijo requiere un monto mayor a cero.");
    }

    private static ConceptoPlanillaDto MapToDto(ConceptoPlanilla c) => new()
    {
        Id = c.Id, Codigo = c.Codigo, Nombre = c.Nombre, Tipo = c.Tipo, MetodoCalculo = c.MetodoCalculo,
        Porcentaje = c.Porcentaje, MontoFijo = c.MontoFijo, AfectaAfp = c.AfectaAfp,
        AfectaEssalud = c.AfectaEssalud, EsSistema = c.EsSistema, Orden = c.Orden
    };
}
