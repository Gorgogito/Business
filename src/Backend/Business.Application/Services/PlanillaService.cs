namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.Common;
using Business.Application.DTOs.Rrhh;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Rrhh;
using Business.Domain.Interfaces;

public class PlanillaService : IPlanillaService
{
    private readonly IRepository<Planilla> _repo;
    private readonly IRepository<Trabajador> _trabajadorRepo;
    private readonly IRepository<ConceptoPlanilla> _conceptoRepo;
    private readonly IRepository<TasaAfp> _tasaAfpRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICorrelativoService _correlativos;
    private readonly IAsientoContableService _asientos;
    private readonly IParametroService _parametros;
    private readonly IConfiguracionContableService _configContable;

    public PlanillaService(
        IRepository<Planilla> repo,
        IRepository<Trabajador> trabajadorRepo,
        IRepository<ConceptoPlanilla> conceptoRepo,
        IRepository<TasaAfp> tasaAfpRepo,
        IUnitOfWork unitOfWork,
        ICorrelativoService correlativos,
        IAsientoContableService asientos,
        IParametroService parametros,
        IConfiguracionContableService configContable)
    {
        _repo = repo;
        _trabajadorRepo = trabajadorRepo;
        _conceptoRepo = conceptoRepo;
        _tasaAfpRepo = tasaAfpRepo;
        _unitOfWork = unitOfWork;
        _correlativos = correlativos;
        _asientos = asientos;
        _parametros = parametros;
        _configContable = configContable;
    }

    public async Task<IEnumerable<PlanillaDto>> GetAllAsync()
    {
        var items = await CargarQuery().OrderByDescending(p => p.Anio).ThenByDescending(p => p.Mes).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<PlanillaDto?> GetByIdAsync(int id)
    {
        var item = await CargarQuery().FirstOrDefaultAsync(p => p.Id == id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<PlanillaDto> ProcesarAsync(int anio, int mes, string userName)
    {
        if (mes < 1 || mes > 12)
            throw new BusinessRuleException("El mes debe estar entre 1 y 12.");
        if (await _repo.ExistsAsync(p => p.Anio == anio && p.Mes == mes && p.Estado != EstadoPlanilla.Anulada))
            throw new BusinessRuleException($"Ya existe una planilla procesada para {mes:00}/{anio}.");

        var trabajadores = (await _trabajadorRepo.FindAsync(t => t.Estado == EstadoTrabajador.Activo)).ToList();
        if (trabajadores.Count == 0)
            throw new BusinessRuleException("No hay trabajadores activos para procesar la planilla.");

        var conceptos = (await _conceptoRepo.GetAllAsync()).ToDictionary(c => c.Codigo);
        var rmv = await _parametros.GetRmvAsync();
        var topeAseguradoAfp = await _parametros.GetTopeAseguradoAfpAsync();
        var tasasAfp = (await _tasaAfpRepo.GetAllAsync())
            .ToDictionary(t => t.Nombre, StringComparer.OrdinalIgnoreCase);

        var numero = await _correlativos.SiguienteAsync("PLANILLA", "PLA");
        var planilla = new Planilla
        {
            Numero = numero, Anio = anio, Mes = mes, FechaProceso = DateTime.UtcNow,
            Estado = EstadoPlanilla.Procesada, IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = userName
        };

        foreach (var trabajador in trabajadores)
        {
            var boleta = CalcularBoleta(trabajador, conceptos, rmv, tasasAfp, topeAseguradoAfp);
            planilla.Detalles.Add(boleta);
        }

        planilla.TotalIngresos = planilla.Detalles.Sum(b => b.TotalIngresos);
        planilla.TotalDescuentos = planilla.Detalles.Sum(b => b.TotalDescuentos);
        planilla.TotalAportes = planilla.Detalles.Sum(b => b.TotalAportes);
        planilla.TotalNeto = planilla.Detalles.Sum(b => b.NetoPagar);

        await _repo.AddAsync(planilla);

        // Asiento contable de la planilla (gasto remuneraciones + aporte empleador
        // / aportes por pagar + remuneraciones por pagar).
        var cuentas = await _configContable.ObtenerMapaAsync();
        var fechaAsiento = new DateTime(anio, mes, DateTime.DaysInMonth(anio, mes));
        await _asientos.GenerarAsientoAsync(
            fechaAsiento, $"Planilla {mes:00}/{anio}", TipoAsiento.Planilla, numero,
            new[]
            {
                (cuentas[ConceptosContables.PlanillaRemuneraciones], planilla.TotalIngresos, 0m),
                (cuentas[ConceptosContables.PlanillaAportes], planilla.TotalAportes, 0m),
                (cuentas[ConceptosContables.PlanillaTributosPorPagar], 0m, planilla.TotalDescuentos + planilla.TotalAportes),
                (cuentas[ConceptosContables.PlanillaRemuneracionesPorPagar], 0m, planilla.TotalNeto)
            },
            userName);

        await _unitOfWork.SaveChangesAsync();
        return MapToDto(planilla);
    }

    /// <summary>Calcula la boleta de un trabajador aplicando los conceptos del catálogo.</summary>
    private static PlanillaDetalle CalcularBoleta(Trabajador t, IReadOnlyDictionary<string, ConceptoPlanilla> conceptos,
        decimal rmv, IReadOnlyDictionary<string, TasaAfp> tasasAfp, decimal topeAseguradoAfp)
    {
        var boleta = new PlanillaDetalle
        {
            TrabajadorId = t.Id, SueldoBasico = t.SueldoBasico, RegimenPension = t.RegimenPension,
            IsActive = true, CreatedAt = DateTime.UtcNow
        };

        // --- Ingresos ---
        var ingresos = new List<(ConceptoPlanilla concepto, decimal monto)>();
        if (conceptos.TryGetValue("SUELDO", out var sueldo))
            ingresos.Add((sueldo, t.SueldoBasico));
        if (t.TieneAsignacionFamiliar && conceptos.TryGetValue("ASIGFAM", out var asig))
            ingresos.Add((asig, Math.Round(rmv * 0.10m, 2, MidpointRounding.AwayFromZero))); // 10% de la RMV

        foreach (var (concepto, monto) in ingresos)
            AgregarConcepto(boleta, concepto.Codigo, concepto.Nombre, TipoConcepto.Ingreso, monto);

        var baseAfp = ingresos.Where(x => x.concepto.AfectaAfp).Sum(x => x.monto);
        var baseEssalud = ingresos.Where(x => x.concepto.AfectaEssalud).Sum(x => x.monto);

        // --- Descuentos previsionales según régimen ---
        if (string.Equals(t.RegimenPension, RegimenPension.Onp, StringComparison.OrdinalIgnoreCase))
        {
            AplicarPorcentual(boleta, conceptos, "ONP", TipoConcepto.Descuento, baseAfp);
        }
        else // AFP
        {
            // Si el trabajador está afiliado a una AFP con tasas registradas se usan sus porcentajes
            // reales (comisión y prima varían por administradora); si no, se cae al % del catálogo.
            // La prima de seguro se calcula sobre la remuneración asegurable con tope (la SBS lo
            // publica periódicamente); el aporte al fondo y la comisión no tienen tope.
            var baseSeguro = Math.Min(baseAfp, topeAseguradoAfp);
            if (t.AfpNombre != null && tasasAfp.TryGetValue(t.AfpNombre, out var tasa))
            {
                AplicarTasa(boleta, conceptos, "AFP_FONDO", baseAfp, tasa.AporteFondo);
                AplicarTasa(boleta, conceptos, "AFP_COMISION", baseAfp, tasa.ComisionFlujo);
                AplicarTasa(boleta, conceptos, "AFP_SEGURO", baseSeguro, tasa.PrimaSeguro);
            }
            else
            {
                AplicarPorcentual(boleta, conceptos, "AFP_FONDO", TipoConcepto.Descuento, baseAfp);
                AplicarPorcentual(boleta, conceptos, "AFP_COMISION", TipoConcepto.Descuento, baseAfp);
                AplicarPorcentual(boleta, conceptos, "AFP_SEGURO", TipoConcepto.Descuento, baseSeguro);
            }
        }

        // --- Aporte del empleador (no afecta el neto) ---
        AplicarPorcentual(boleta, conceptos, "ESSALUD", TipoConcepto.Aporte, baseEssalud);

        boleta.TotalIngresos = boleta.Conceptos.Where(c => c.Tipo == TipoConcepto.Ingreso).Sum(c => c.Monto);
        boleta.TotalDescuentos = boleta.Conceptos.Where(c => c.Tipo == TipoConcepto.Descuento).Sum(c => c.Monto);
        boleta.TotalAportes = boleta.Conceptos.Where(c => c.Tipo == TipoConcepto.Aporte).Sum(c => c.Monto);
        boleta.NetoPagar = boleta.TotalIngresos - boleta.TotalDescuentos;
        return boleta;
    }

    private static void AplicarPorcentual(PlanillaDetalle boleta, IReadOnlyDictionary<string, ConceptoPlanilla> conceptos,
        string codigo, string tipo, decimal baseCalculo)
    {
        if (!conceptos.TryGetValue(codigo, out var concepto)) return;
        var monto = Math.Round(baseCalculo * (concepto.Porcentaje ?? 0), 2, MidpointRounding.AwayFromZero);
        if (monto > 0) AgregarConcepto(boleta, concepto.Codigo, concepto.Nombre, tipo, monto);
    }

    /// <summary>Aplica un descuento AFP con un porcentaje explícito (tasa real de la administradora).</summary>
    private static void AplicarTasa(PlanillaDetalle boleta, IReadOnlyDictionary<string, ConceptoPlanilla> conceptos,
        string codigo, decimal baseCalculo, decimal porcentaje)
    {
        var nombre = conceptos.TryGetValue(codigo, out var c) ? c.Nombre : codigo;
        var monto = Math.Round(baseCalculo * porcentaje, 2, MidpointRounding.AwayFromZero);
        if (monto > 0) AgregarConcepto(boleta, codigo, nombre, TipoConcepto.Descuento, monto);
    }

    private static void AgregarConcepto(PlanillaDetalle boleta, string codigo, string nombre, string tipo, decimal monto)
        => boleta.Conceptos.Add(new PlanillaConcepto
        {
            ConceptoCodigo = codigo, ConceptoNombre = nombre, Tipo = tipo,
            Monto = Math.Round(monto, 2, MidpointRounding.AwayFromZero),
            IsActive = true, CreatedAt = DateTime.UtcNow
        });

    public async Task<PlanillaDto?> AnularAsync(int id, string userName)
    {
        var planilla = await CargarQuery().FirstOrDefaultAsync(p => p.Id == id);
        if (planilla == null) return null;
        if (planilla.Estado == EstadoPlanilla.Anulada)
            throw new BusinessRuleException("La planilla ya está anulada.");

        planilla.Estado = EstadoPlanilla.Anulada;
        planilla.UpdatedAt = DateTime.UtcNow; planilla.UpdatedBy = userName;

        // Asiento de reversión (invierte el asiento de provisión de la planilla).
        var cuentas = await _configContable.ObtenerMapaAsync();
        var fechaAsiento = new DateTime(planilla.Anio, planilla.Mes, DateTime.DaysInMonth(planilla.Anio, planilla.Mes));
        await _asientos.GenerarAsientoAsync(
            fechaAsiento, $"Anulación planilla {planilla.Mes:00}/{planilla.Anio}", TipoAsiento.Planilla, $"ANU-{planilla.Numero}",
            new[]
            {
                (cuentas[ConceptosContables.PlanillaRemuneraciones], 0m, planilla.TotalIngresos),
                (cuentas[ConceptosContables.PlanillaAportes], 0m, planilla.TotalAportes),
                (cuentas[ConceptosContables.PlanillaTributosPorPagar], planilla.TotalDescuentos + planilla.TotalAportes, 0m),
                (cuentas[ConceptosContables.PlanillaRemuneracionesPorPagar], planilla.TotalNeto, 0m)
            },
            userName);

        await _unitOfWork.SaveChangesAsync();
        return MapToDto(planilla);
    }

    public async Task<PlanillaBoletaDto?> GetBoletaAsync(int planillaId, int trabajadorId)
    {
        var planilla = await CargarQuery().FirstOrDefaultAsync(p => p.Id == planillaId);
        var boleta = planilla?.Detalles.FirstOrDefault(b => b.TrabajadorId == trabajadorId);
        return boleta != null ? MapToBoleta(boleta) : null;
    }

    public async Task<IEnumerable<PlanillaBoletaDto>> GetBoletasPorTrabajadorAsync(int trabajadorId)
    {
        var planillas = await CargarQuery()
            .Where(p => p.Estado != EstadoPlanilla.Anulada && p.Detalles.Any(b => b.TrabajadorId == trabajadorId))
            .OrderByDescending(p => p.Anio).ThenByDescending(p => p.Mes)
            .ToListAsync();
        return planillas
            .Select(p => p.Detalles.First(b => b.TrabajadorId == trabajadorId))
            .Select(MapToBoleta);
    }

    private IQueryable<Planilla> CargarQuery() => _repo.Query()
        .Include(p => p.Detalles).ThenInclude(b => b.Trabajador)
        .Include(p => p.Detalles).ThenInclude(b => b.Conceptos);

    private static PlanillaDto MapToDto(Planilla p) => new()
    {
        Id = p.Id, Numero = p.Numero, Anio = p.Anio, Mes = p.Mes, FechaProceso = p.FechaProceso, Estado = p.Estado,
        TotalIngresos = p.TotalIngresos, TotalDescuentos = p.TotalDescuentos, TotalAportes = p.TotalAportes, TotalNeto = p.TotalNeto,
        Boletas = p.Detalles.Select(MapToBoleta).ToList()
    };

    private static PlanillaBoletaDto MapToBoleta(PlanillaDetalle b) => new()
    {
        Id = b.Id, TrabajadorId = b.TrabajadorId,
        TrabajadorCodigo = b.Trabajador?.Codigo,
        TrabajadorNombre = b.Trabajador == null ? null : $"{b.Trabajador.ApellidoPaterno} {b.Trabajador.ApellidoMaterno}, {b.Trabajador.Nombres}",
        SueldoBasico = b.SueldoBasico, RegimenPension = b.RegimenPension,
        TotalIngresos = b.TotalIngresos, TotalDescuentos = b.TotalDescuentos, TotalAportes = b.TotalAportes, NetoPagar = b.NetoPagar,
        Conceptos = b.Conceptos.Select(c => new PlanillaConceptoDto
        {
            ConceptoCodigo = c.ConceptoCodigo, ConceptoNombre = c.ConceptoNombre, Tipo = c.Tipo, Monto = c.Monto
        }).ToList()
    };
}
