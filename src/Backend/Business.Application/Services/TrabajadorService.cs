namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.Common;
using Business.Application.DTOs.Rrhh;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Rrhh;
using Business.Domain.Interfaces;

public class TrabajadorService : ITrabajadorService
{
    private readonly IRepository<Trabajador> _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICorrelativoService _correlativos;

    public TrabajadorService(IRepository<Trabajador> repo, IUnitOfWork unitOfWork, ICorrelativoService correlativos)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _correlativos = correlativos;
    }

    public async Task<IEnumerable<TrabajadorDto>> GetAllAsync()
    {
        var items = await _repo.Query().OrderBy(t => t.ApellidoPaterno).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<TrabajadorDto>> GetActivosAsync()
    {
        var items = await _repo.Query().Where(t => t.Estado == EstadoTrabajador.Activo).OrderBy(t => t.ApellidoPaterno).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<TrabajadorDto?> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<TrabajadorDto> CreateAsync(CreateTrabajadorDto dto, string userName)
    {
        if (await _repo.ExistsAsync(t => t.NumeroDocumento == dto.NumeroDocumento))
            throw new BusinessRuleException($"Ya existe un trabajador con el documento {dto.NumeroDocumento}.");

        ValidarRegimen(dto);

        var codigo = await _correlativos.SiguienteAsync("TRABAJADOR", "T");
        var entity = new Trabajador
        {
            Codigo = codigo,
            TipoDocumento = dto.TipoDocumento, NumeroDocumento = dto.NumeroDocumento,
            Nombres = dto.Nombres, ApellidoPaterno = dto.ApellidoPaterno, ApellidoMaterno = dto.ApellidoMaterno,
            FechaNacimiento = dto.FechaNacimiento, Sexo = dto.Sexo, Direccion = dto.Direccion,
            Telefono = dto.Telefono, Email = dto.Email, Cargo = dto.Cargo,
            FechaIngreso = dto.FechaIngreso == default ? DateTime.UtcNow : dto.FechaIngreso,
            TipoContrato = dto.TipoContrato, SueldoBasico = dto.SueldoBasico, TieneAsignacionFamiliar = dto.TieneAsignacionFamiliar,
            Estado = EstadoTrabajador.Activo,
            RegimenPension = dto.RegimenPension, AfpNombre = dto.AfpNombre, Cuspp = dto.Cuspp,
            IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = userName
        };
        await _repo.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<TrabajadorDto?> UpdateAsync(int id, CreateTrabajadorDto dto, string userName)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return null;

        if (await _repo.ExistsAsync(t => t.NumeroDocumento == dto.NumeroDocumento && t.Id != id))
            throw new BusinessRuleException($"Ya existe otro trabajador con el documento {dto.NumeroDocumento}.");

        ValidarRegimen(dto);

        entity.TipoDocumento = dto.TipoDocumento; entity.NumeroDocumento = dto.NumeroDocumento;
        entity.Nombres = dto.Nombres; entity.ApellidoPaterno = dto.ApellidoPaterno; entity.ApellidoMaterno = dto.ApellidoMaterno;
        entity.FechaNacimiento = dto.FechaNacimiento; entity.Sexo = dto.Sexo; entity.Direccion = dto.Direccion;
        entity.Telefono = dto.Telefono; entity.Email = dto.Email; entity.Cargo = dto.Cargo;
        entity.FechaIngreso = dto.FechaIngreso; entity.TipoContrato = dto.TipoContrato; entity.SueldoBasico = dto.SueldoBasico;
        entity.TieneAsignacionFamiliar = dto.TieneAsignacionFamiliar;
        entity.RegimenPension = dto.RegimenPension; entity.AfpNombre = dto.AfpNombre; entity.Cuspp = dto.Cuspp;
        entity.UpdatedAt = DateTime.UtcNow; entity.UpdatedBy = userName;
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<TrabajadorDto?> CesarAsync(int id, DateTime fechaCese, string userName)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return null;

        entity.Estado = EstadoTrabajador.Cesado;
        entity.FechaCese = fechaCese;
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

    private static void ValidarRegimen(CreateTrabajadorDto dto)
    {
        if (string.Equals(dto.RegimenPension, RegimenPension.Afp, StringComparison.OrdinalIgnoreCase)
            && string.IsNullOrWhiteSpace(dto.AfpNombre))
        {
            throw new BusinessRuleException("Debe indicar la AFP para un trabajador afiliado al régimen AFP.");
        }
    }

    private static TrabajadorDto MapToDto(Trabajador t) => new()
    {
        Id = t.Id, Codigo = t.Codigo, TipoDocumento = t.TipoDocumento, NumeroDocumento = t.NumeroDocumento,
        Nombres = t.Nombres, ApellidoPaterno = t.ApellidoPaterno, ApellidoMaterno = t.ApellidoMaterno,
        NombreCompleto = $"{t.ApellidoPaterno} {t.ApellidoMaterno}, {t.Nombres}".Trim(),
        FechaNacimiento = t.FechaNacimiento, Sexo = t.Sexo, Direccion = t.Direccion, Telefono = t.Telefono, Email = t.Email,
        Cargo = t.Cargo, FechaIngreso = t.FechaIngreso, FechaCese = t.FechaCese, TipoContrato = t.TipoContrato,
        SueldoBasico = t.SueldoBasico, TieneAsignacionFamiliar = t.TieneAsignacionFamiliar,
        Estado = t.Estado, RegimenPension = t.RegimenPension,
        AfpNombre = t.AfpNombre, Cuspp = t.Cuspp
    };
}
