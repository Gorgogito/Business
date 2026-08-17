namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.Common;
using Business.Application.DTOs.Contabilidad;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Contabilidad;
using Business.Domain.Interfaces;

public class ConfiguracionContableService : IConfiguracionContableService
{
    private readonly IRepository<ConfiguracionCuentaContable> _repo;
    private readonly IRepository<CuentaContable> _cuentaRepo;
    private readonly IUnitOfWork _unitOfWork;

    public ConfiguracionContableService(
        IRepository<ConfiguracionCuentaContable> repo,
        IRepository<CuentaContable> cuentaRepo,
        IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _cuentaRepo = cuentaRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ConfiguracionCuentaContableDto>> GetAllAsync()
    {
        var configurados = await _repo.Query().Include(c => c.CuentaContable).ToDictionaryAsync(c => c.Concepto);
        var cuentasPorCodigo = (await _cuentaRepo.GetAllAsync()).ToDictionary(c => c.Codigo);

        return ConceptosContables.Catalogo.Select(item =>
        {
            if (configurados.TryGetValue(item.Concepto, out var cfg))
            {
                return new ConfiguracionCuentaContableDto
                {
                    Concepto = item.Concepto, Modulo = item.Modulo, Descripcion = item.Descripcion,
                    CuentaCodigoDefecto = item.CuentaDefecto,
                    CuentaContableId = cfg.CuentaContableId, CuentaCodigo = cfg.CuentaContable.Codigo,
                    CuentaNombre = cfg.CuentaContable.Nombre, EsPersonalizado = true
                };
            }

            cuentasPorCodigo.TryGetValue(item.CuentaDefecto, out var cuentaDefecto);
            return new ConfiguracionCuentaContableDto
            {
                Concepto = item.Concepto, Modulo = item.Modulo, Descripcion = item.Descripcion,
                CuentaCodigoDefecto = item.CuentaDefecto,
                CuentaContableId = cuentaDefecto?.Id, CuentaCodigo = cuentaDefecto?.Codigo,
                CuentaNombre = cuentaDefecto?.Nombre, EsPersonalizado = false
            };
        }).ToList();
    }

    public async Task<ConfiguracionCuentaContableDto> ConfigurarAsync(string concepto, int cuentaContableId, string userName)
    {
        var item = ConceptosContables.Catalogo.FirstOrDefault(c => c.Concepto == concepto);
        if (item.Concepto == null)
            throw new BusinessRuleException($"El concepto '{concepto}' no es reconocido por el sistema.");

        var cuenta = await _cuentaRepo.GetByIdAsync(cuentaContableId)
            ?? throw new BusinessRuleException($"La cuenta contable {cuentaContableId} no existe.");
        if (!cuenta.EsMovimiento)
            throw new BusinessRuleException($"La cuenta {cuenta.Codigo} - {cuenta.Nombre} no admite movimientos (es de agrupación).");

        var existente = await _repo.Query().FirstOrDefaultAsync(c => c.Concepto == concepto);
        if (existente == null)
        {
            existente = new ConfiguracionCuentaContable
            {
                Concepto = concepto, Modulo = item.Modulo, Descripcion = item.Descripcion,
                CuentaContableId = cuentaContableId, IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = userName
            };
            await _repo.AddAsync(existente);
        }
        else
        {
            existente.CuentaContableId = cuentaContableId;
            existente.UpdatedAt = DateTime.UtcNow;
            existente.UpdatedBy = userName;
        }

        await _unitOfWork.SaveChangesAsync();
        return new ConfiguracionCuentaContableDto
        {
            Concepto = concepto, Modulo = item.Modulo, Descripcion = item.Descripcion,
            CuentaCodigoDefecto = item.CuentaDefecto,
            CuentaContableId = cuenta.Id, CuentaCodigo = cuenta.Codigo, CuentaNombre = cuenta.Nombre, EsPersonalizado = true
        };
    }

    public async Task<IReadOnlyDictionary<string, string>> ObtenerMapaAsync(CancellationToken ct = default)
    {
        var mapa = await _repo.Query().Include(c => c.CuentaContable)
            .ToDictionaryAsync(c => c.Concepto, c => c.CuentaContable.Codigo, ct);

        foreach (var (concepto, codigo) in ConceptosContables.Defaults)
            mapa.TryAdd(concepto, codigo);

        return mapa;
    }
}
