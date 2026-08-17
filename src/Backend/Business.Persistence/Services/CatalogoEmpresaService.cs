namespace Business.Persistence.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.Interfaces;
using Business.Persistence.Context;

/// <summary>
/// Clona los catálogos base (plan de cuentas y conceptos de planilla) de la empresa base
/// hacia una empresa nueva mediante INSERT ... SELECT, sin interferir con el ChangeTracker.
/// </summary>
public class CatalogoEmpresaService : ICatalogoEmpresaService
{
    private const int EmpresaBase = 1;
    private readonly ApplicationDbContext _context;

    public CatalogoEmpresaService(ApplicationDbContext context) => _context = context;

    public async Task ClonarCatalogoBaseAsync(int empresaId, CancellationToken ct = default)
    {
        if (empresaId == EmpresaBase) return; // la empresa base ya trae el catálogo sembrado

        // Idempotente por tabla: cada una se clona solo si la empresa aún no la tiene, para que
        // volver a llamar este método (re-aprovisionar) complete lo que falte sin duplicar nada.
        var yaTieneCuentas = await _context.CuentasContables.IgnoreQueryFilters()
            .AnyAsync(c => c.EmpresaId == empresaId, ct);
        if (!yaTieneCuentas)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"INSERT INTO CuentasContables (EmpresaId, Codigo, Nombre, Clase, Naturaleza, Nivel, EsMovimiento, IsActive, CreatedAt)
                  SELECT {0}, Codigo, Nombre, Clase, Naturaleza, Nivel, EsMovimiento, 1, SYSUTCDATETIME()
                  FROM CuentasContables WHERE EmpresaId = {1} AND IsActive = 1",
                new object[] { empresaId, EmpresaBase }, ct);
        }

        var yaTieneConceptos = await _context.ConceptosPlanilla.IgnoreQueryFilters()
            .AnyAsync(c => c.EmpresaId == empresaId, ct);
        if (!yaTieneConceptos)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"INSERT INTO ConceptosPlanilla (EmpresaId, Codigo, Nombre, Tipo, MetodoCalculo, Porcentaje, MontoFijo, AfectaAfp, AfectaEssalud, EsSistema, Orden, IsActive, CreatedAt)
                  SELECT {0}, Codigo, Nombre, Tipo, MetodoCalculo, Porcentaje, MontoFijo, AfectaAfp, AfectaEssalud, EsSistema, Orden, 1, SYSUTCDATETIME()
                  FROM ConceptosPlanilla WHERE EmpresaId = {1} AND IsActive = 1",
                new object[] { empresaId, EmpresaBase }, ct);
        }

        // La configuración de cuentas por concepto referencia CuentaContableId, que difiere por
        // empresa (son filas clonadas con IDs propios): se resuelve haciendo join por Código
        // contra el plan de la empresa destino (que a esta altura ya existe, recién clonado o de antes).
        var yaTieneConfig = await _context.ConfiguracionesCuentasContables.IgnoreQueryFilters()
            .AnyAsync(c => c.EmpresaId == empresaId, ct);
        if (!yaTieneConfig)
        {
            await _context.Database.ExecuteSqlRawAsync(
                @"INSERT INTO ConfiguracionesCuentasContables (EmpresaId, Concepto, Modulo, Descripcion, CuentaContableId, IsActive, CreatedAt)
                  SELECT {0}, cfgBase.Concepto, cfgBase.Modulo, cfgBase.Descripcion, ctaDestino.Id, 1, SYSUTCDATETIME()
                  FROM ConfiguracionesCuentasContables cfgBase
                  JOIN CuentasContables ctaBase ON ctaBase.Id = cfgBase.CuentaContableId
                  JOIN CuentasContables ctaDestino ON ctaDestino.EmpresaId = {0} AND ctaDestino.Codigo = ctaBase.Codigo
                  WHERE cfgBase.EmpresaId = {1} AND cfgBase.IsActive = 1",
                new object[] { empresaId, EmpresaBase }, ct);
        }
    }
}
