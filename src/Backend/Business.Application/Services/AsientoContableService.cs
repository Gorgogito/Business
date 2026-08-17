namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.Common;
using Business.Application.DTOs.Contabilidad;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Contabilidad;
using Business.Domain.Interfaces;

public class AsientoContableService : IAsientoContableService
{
    private readonly IRepository<AsientoContable> _repo;
    private readonly IRepository<CuentaContable> _cuentaRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICorrelativoService _correlativos;

    public AsientoContableService(
        IRepository<AsientoContable> repo,
        IRepository<CuentaContable> cuentaRepo,
        IUnitOfWork unitOfWork,
        ICorrelativoService correlativos)
    {
        _repo = repo;
        _cuentaRepo = cuentaRepo;
        _unitOfWork = unitOfWork;
        _correlativos = correlativos;
    }

    public async Task<IEnumerable<AsientoContableDto>> GetAllAsync()
    {
        var items = await _repo.Query().Include(a => a.Detalles).ThenInclude(d => d.CuentaContable)
            .OrderByDescending(a => a.Fecha).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<AsientoContableDto?> GetByIdAsync(int id)
    {
        var item = await _repo.Query().Include(a => a.Detalles).ThenInclude(d => d.CuentaContable)
            .FirstOrDefaultAsync(a => a.Id == id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<IEnumerable<AsientoContableDto>> GetByPeriodoAsync(DateTime desde, DateTime hasta)
    {
        var d = desde.Date;
        var h = hasta.Date.AddDays(1).AddTicks(-1);
        var items = await _repo.Query().Include(a => a.Detalles).ThenInclude(d => d.CuentaContable)
            .Where(a => a.Fecha >= d && a.Fecha <= h).OrderBy(a => a.Fecha).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<AsientoContableDto> CreateAsync(CreateAsientoContableDto dto, string userName)
    {
        if (dto.Detalles.Count < 2)
            throw new BusinessRuleException("El asiento debe tener al menos dos líneas.");

        // Validar cuentas: existen y admiten movimiento.
        var cuentaIds = dto.Detalles.Select(x => x.CuentaContableId).Distinct().ToList();
        var cuentas = (await _cuentaRepo.FindAsync(c => cuentaIds.Contains(c.Id))).ToDictionary(c => c.Id);
        foreach (var id in cuentaIds)
        {
            if (!cuentas.TryGetValue(id, out var cuenta))
                throw new BusinessRuleException($"La cuenta contable {id} no existe.");
            if (!cuenta.EsMovimiento)
                throw new BusinessRuleException($"La cuenta {cuenta.Codigo} - {cuenta.Nombre} no admite movimientos (es de agrupación).");
        }

        decimal totalDebe = 0, totalHaber = 0;
        foreach (var linea in dto.Detalles)
        {
            var tieneDebe = linea.Debe > 0;
            var tieneHaber = linea.Haber > 0;
            if (tieneDebe == tieneHaber) // ambos o ninguno
                throw new BusinessRuleException("Cada línea debe tener importe solo en el debe o solo en el haber.");
            totalDebe += linea.Debe;
            totalHaber += linea.Haber;
        }

        if (Math.Abs(totalDebe - totalHaber) > 0.005m)
            throw new BusinessRuleException($"El asiento no cuadra: debe {totalDebe:0.00} ≠ haber {totalHaber:0.00}.");

        var numero = await _correlativos.SiguienteAsync("ASIENTO", "ASI");
        var asiento = new AsientoContable
        {
            Numero = numero, Fecha = dto.Fecha == default ? DateTime.UtcNow : dto.Fecha,
            Glosa = dto.Glosa, Tipo = TipoAsiento.Diario, Estado = EstadoAsiento.Registrado,
            TotalDebe = totalDebe, TotalHaber = totalHaber,
            IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = userName
        };
        foreach (var linea in dto.Detalles)
        {
            asiento.Detalles.Add(new AsientoContableDetalle
            {
                CuentaContableId = linea.CuentaContableId, Debe = linea.Debe, Haber = linea.Haber,
                Glosa = linea.Glosa, IsActive = true, CreatedAt = DateTime.UtcNow
            });
        }

        await _repo.AddAsync(asiento);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(asiento);
    }

    public async Task GenerarAsientoAsync(
        DateTime fecha, string glosa, string tipo, string? referencia,
        IEnumerable<(string CuentaCodigo, decimal Debe, decimal Haber)> lineas,
        string? userName, CancellationToken ct = default)
    {
        // Ignora líneas sin importe (p. ej. costo de ventas 0 en productos sin costo).
        var activas = lineas.Where(l => l.Debe != 0 || l.Haber != 0).ToList();
        if (activas.Count < 2) return;

        var codigos = activas.Select(l => l.CuentaCodigo).Distinct().ToList();
        var cuentas = (await _cuentaRepo.FindAsync(c => codigos.Contains(c.Codigo))).ToDictionary(c => c.Codigo);
        foreach (var cod in codigos)
        {
            if (!cuentas.TryGetValue(cod, out var cuenta))
                throw new BusinessRuleException($"La cuenta contable {cod} no está configurada en el plan.");
            if (!cuenta.EsMovimiento)
                throw new BusinessRuleException($"La cuenta {cuenta.Codigo} no admite movimientos.");
        }

        var totalDebe = activas.Sum(l => l.Debe);
        var totalHaber = activas.Sum(l => l.Haber);
        if (Math.Abs(totalDebe - totalHaber) > 0.005m)
            throw new BusinessRuleException($"El asiento automático no cuadra: debe {totalDebe:0.00} ≠ haber {totalHaber:0.00}.");

        var numero = await _correlativos.SiguienteAsync("ASIENTO", "ASI");
        var asiento = new AsientoContable
        {
            Numero = numero, Fecha = fecha, Glosa = glosa, Tipo = tipo, Referencia = referencia,
            TotalDebe = totalDebe, TotalHaber = totalHaber, Estado = EstadoAsiento.Registrado,
            IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = userName
        };
        foreach (var l in activas)
        {
            asiento.Detalles.Add(new AsientoContableDetalle
            {
                CuentaContableId = cuentas[l.CuentaCodigo].Id, Debe = l.Debe, Haber = l.Haber,
                IsActive = true, CreatedAt = DateTime.UtcNow
            });
        }
        await _repo.AddAsync(asiento);
    }

    public async Task<AsientoContableDto?> AnularAsync(int id, string userName)
    {
        var asiento = await _repo.Query().Include(a => a.Detalles).ThenInclude(d => d.CuentaContable)
            .FirstOrDefaultAsync(a => a.Id == id);
        if (asiento == null) return null;

        if (asiento.Estado == EstadoAsiento.Anulado)
            throw new BusinessRuleException("El asiento ya está anulado.");

        asiento.Estado = EstadoAsiento.Anulado;
        asiento.UpdatedAt = DateTime.UtcNow;
        asiento.UpdatedBy = userName;
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(asiento);
    }

    private static AsientoContableDto MapToDto(AsientoContable a) => new()
    {
        Id = a.Id, Numero = a.Numero, Fecha = a.Fecha, Glosa = a.Glosa, Tipo = a.Tipo,
        Referencia = a.Referencia, TotalDebe = a.TotalDebe, TotalHaber = a.TotalHaber, Estado = a.Estado,
        Detalles = a.Detalles.Select(d => new AsientoDetalleDto
        {
            Id = d.Id, CuentaContableId = d.CuentaContableId,
            CuentaCodigo = d.CuentaContable?.Codigo, CuentaNombre = d.CuentaContable?.Nombre,
            Debe = d.Debe, Haber = d.Haber, Glosa = d.Glosa
        }).ToList()
    };
}
