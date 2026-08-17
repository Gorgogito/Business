namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.Common;
using Business.Application.DTOs.Rrhh;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Rrhh;
using Business.Domain.Interfaces;

/// <summary>
/// Calcula los beneficios sociales del régimen laboral general privado: CTS, gratificaciones
/// (con bonificación extraordinaria del 9%, Ley 30334) y remuneración vacacional. La remuneración
/// computable base es el sueldo más la asignación familiar (10% de la RMV) cuando corresponde.
/// </summary>
public class BeneficioSocialService : IBeneficioSocialService
{
    private readonly IRepository<BeneficioSocial> _repo;
    private readonly IRepository<Trabajador> _trabajadorRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IParametroService _parametros;
    private readonly IAsientoContableService _asientos;
    private readonly IConfiguracionContableService _configContable;

    public BeneficioSocialService(
        IRepository<BeneficioSocial> repo,
        IRepository<Trabajador> trabajadorRepo,
        IUnitOfWork unitOfWork,
        IParametroService parametros,
        IAsientoContableService asientos,
        IConfiguracionContableService configContable)
    {
        _repo = repo;
        _trabajadorRepo = trabajadorRepo;
        _unitOfWork = unitOfWork;
        _parametros = parametros;
        _asientos = asientos;
        _configContable = configContable;
    }

    public async Task<IEnumerable<BeneficioSocialDto>> GetAllAsync()
    {
        var items = await CargarQuery().OrderByDescending(b => b.FechaCalculo).ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<BeneficioSocialDto>> GetByTrabajadorAsync(int trabajadorId)
    {
        var items = await CargarQuery().Where(b => b.TrabajadorId == trabajadorId)
            .OrderByDescending(b => b.FechaCalculo).ToListAsync();
        return items.Select(MapToDto);
    }

    public Task<IEnumerable<BeneficioSocialDto>> CalcularCtsAsync(int anio, int semestre, string userName)
    {
        // semestre 1: may–oct del año (depósito noviembre); semestre 2: nov del año a abr del siguiente (depósito mayo).
        var (desde, hasta) = semestre switch
        {
            1 => (new DateTime(anio, 5, 1), new DateTime(anio, 10, 31)),
            2 => (new DateTime(anio, 11, 1), new DateTime(anio + 1, 4, 30)),
            _ => throw new BusinessRuleException("El semestre de CTS debe ser 1 (may-oct) o 2 (nov-abr).")
        };
        var periodo = $"{anio}-{semestre}";
        return ProcesarAsync(TipoBeneficio.Cts, periodo, desde, hasta, userName, (remBase, meses) =>
        {
            // Remuneración computable = sueldo + asig. familiar + 1/6 de la última gratificación (≈ remBase).
            var remComputable = Math.Round(remBase * (1m + 1m / 6m), 2, MidpointRounding.AwayFromZero);
            var monto = Math.Round(remComputable * meses / 12m, 2, MidpointRounding.AwayFromZero);
            return (remComputable, monto, 0m, $"CTS semestre {semestre}/{anio} ({desde:dd/MM/yyyy}-{hasta:dd/MM/yyyy})");
        });
    }

    public Task<IEnumerable<BeneficioSocialDto>> CalcularGratificacionAsync(int anio, int semestre, string userName)
    {
        // semestre 1: ene–jun (Fiestas Patrias, pago julio); semestre 2: jul–dic (Navidad, pago diciembre).
        var (desde, hasta, etiqueta) = semestre switch
        {
            1 => (new DateTime(anio, 1, 1), new DateTime(anio, 6, 30), "JUL"),
            2 => (new DateTime(anio, 7, 1), new DateTime(anio, 12, 31), "DIC"),
            _ => throw new BusinessRuleException("El semestre de gratificación debe ser 1 (ene-jun) o 2 (jul-dic).")
        };
        var periodo = $"{anio}-{etiqueta}";
        return ProcesarAsync(TipoBeneficio.Gratificacion, periodo, desde, hasta, userName, (remBase, meses) =>
        {
            var monto = Math.Round(remBase * meses / 6m, 2, MidpointRounding.AwayFromZero);
            // Bonificación extraordinaria: 9% del aporte que iría a EsSalud (Ley 30334).
            var bonif = Math.Round(monto * 0.09m, 2, MidpointRounding.AwayFromZero);
            return (remBase, monto, bonif, $"Gratificación {etiqueta} {anio} ({desde:dd/MM/yyyy}-{hasta:dd/MM/yyyy})");
        });
    }

    public Task<IEnumerable<BeneficioSocialDto>> CalcularVacacionesAsync(int anio, string userName)
    {
        var desde = new DateTime(anio, 1, 1);
        var hasta = new DateTime(anio, 12, 31);
        var periodo = anio.ToString();
        return ProcesarAsync(TipoBeneficio.Vacaciones, periodo, desde, hasta, userName, (remBase, meses) =>
        {
            // Récord vacacional anual: 30 días = 1 remuneración por 12 meses (proporcional).
            var monto = Math.Round(remBase * meses / 12m, 2, MidpointRounding.AwayFromZero);
            return (remBase, monto, 0m, $"Remuneración vacacional {anio} (récord {meses:0.##} meses)");
        });
    }

    /// <summary>Núcleo común: valida duplicado, calcula por trabajador activo y persiste.</summary>
    private async Task<IEnumerable<BeneficioSocialDto>> ProcesarAsync(
        string tipo, string periodo, DateTime desde, DateTime hasta, string userName,
        Func<decimal, decimal, (decimal remComputable, decimal monto, decimal bonif, string observacion)> calcular)
    {
        if (await _repo.ExistsAsync(b => b.Tipo == tipo && b.Periodo == periodo))
            throw new BusinessRuleException($"Ya se calculó {tipo} para el período {periodo}.");

        var trabajadores = (await _trabajadorRepo.FindAsync(t => t.Estado == EstadoTrabajador.Activo)).ToList();
        if (trabajadores.Count == 0)
            throw new BusinessRuleException("No hay trabajadores activos para calcular el beneficio.");

        var rmv = await _parametros.GetRmvAsync();
        int topeMeses = tipo == TipoBeneficio.Gratificacion ? 6 : 12;
        var creados = new List<BeneficioSocial>();

        foreach (var t in trabajadores)
        {
            var meses = MesesComputables(desde, hasta, t.FechaIngreso, topeMeses);
            if (meses <= 0) continue; // ingresó después del período: no le corresponde

            var asigFam = t.TieneAsignacionFamiliar ? Math.Round(rmv * 0.10m, 2, MidpointRounding.AwayFromZero) : 0m;
            var remBase = t.SueldoBasico + asigFam;
            var (remComputable, monto, bonif, observacion) = calcular(remBase, meses);

            var beneficio = new BeneficioSocial
            {
                TrabajadorId = t.Id, Trabajador = t, Tipo = tipo, Periodo = periodo, FechaCalculo = DateTime.UtcNow,
                RemuneracionComputable = remComputable, MesesComputables = meses,
                Monto = monto, BonificacionExtraordinaria = bonif, Observacion = observacion,
                IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = userName
            };
            await _repo.AddAsync(beneficio);
            creados.Add(beneficio);
        }

        if (creados.Count == 0)
            throw new BusinessRuleException("Ningún trabajador activo tiene meses computables en el período indicado.");

        // Asiento de provisión: gasto por beneficios sociales / beneficios sociales por pagar.
        var totalProvision = creados.Sum(b => b.Monto + b.BonificacionExtraordinaria);
        var cuentas = await _configContable.ObtenerMapaAsync();
        await _asientos.GenerarAsientoAsync(
            hasta, $"Provisión {tipo} {periodo}", TipoAsiento.Beneficios, periodo,
            new[]
            {
                (cuentas[ConceptosContables.PlanillaBeneficiosGasto], totalProvision, 0m),
                (cuentas[ConceptosContables.PlanillaBeneficiosPorPagar], 0m, totalProvision)
            },
            userName);

        await _unitOfWork.SaveChangesAsync();
        return creados.Select(MapToDto);
    }

    public async Task<BeneficioSocialDto?> RegistrarPagoAsync(int id, string medioPago, string userName)
    {
        var beneficio = (await _repo.FindAsync(b => b.Id == id)).FirstOrDefault();
        if (beneficio == null) return null;

        if (beneficio.EstadoPago == EstadoPagoBeneficio.Pagado)
            throw new BusinessRuleException("El beneficio ya fue pagado.");

        var totalPagar = beneficio.Monto + beneficio.BonificacionExtraordinaria;
        var medio = string.IsNullOrWhiteSpace(medioPago) ? "EFECTIVO" : medioPago;

        // Asiento de pago: 413 beneficios sociales por pagar (debe) / caja-bancos (haber).
        var cuentas = await _configContable.ObtenerMapaAsync();
        var cuentaCaja = string.Equals(medio, "EFECTIVO", StringComparison.OrdinalIgnoreCase)
            ? cuentas[ConceptosContables.TesoreriaEfectivo] : cuentas[ConceptosContables.TesoreriaBancos];
        await _asientos.GenerarAsientoAsync(
            DateTime.UtcNow, $"Pago {beneficio.Tipo} {beneficio.Periodo}", TipoAsiento.Pago, $"BEN-{beneficio.Id}",
            new[] { (cuentas[ConceptosContables.PlanillaBeneficiosPorPagar], totalPagar, 0m), (cuentaCaja, 0m, totalPagar) },
            userName);

        beneficio.EstadoPago = EstadoPagoBeneficio.Pagado;
        beneficio.FechaPago = DateTime.UtcNow;
        beneficio.MedioPago = medio;
        beneficio.UpdatedAt = DateTime.UtcNow;
        beneficio.UpdatedBy = userName;

        await _unitOfWork.SaveChangesAsync();

        beneficio.Trabajador ??= await _trabajadorRepo.GetByIdAsync(beneficio.TrabajadorId);
        return MapToDto(beneficio);
    }

    /// <summary>Meses completos trabajados dentro del rango (incluye el mes de ingreso), con tope.</summary>
    private static decimal MesesComputables(DateTime desde, DateTime hasta, DateTime ingreso, int tope)
    {
        var inicio = ingreso > desde ? ingreso : desde;
        if (inicio > hasta) return 0;
        var meses = (hasta.Year - inicio.Year) * 12 + (hasta.Month - inicio.Month) + 1;
        if (meses < 0) meses = 0;
        if (meses > tope) meses = tope;
        return meses;
    }

    private IQueryable<BeneficioSocial> CargarQuery() => _repo.Query().Include(b => b.Trabajador);

    private static BeneficioSocialDto MapToDto(BeneficioSocial b) => new()
    {
        Id = b.Id, TrabajadorId = b.TrabajadorId,
        TrabajadorCodigo = b.Trabajador?.Codigo,
        TrabajadorNombre = b.Trabajador == null ? null : $"{b.Trabajador.ApellidoPaterno} {b.Trabajador.ApellidoMaterno}, {b.Trabajador.Nombres}",
        Tipo = b.Tipo, Periodo = b.Periodo, FechaCalculo = b.FechaCalculo,
        RemuneracionComputable = b.RemuneracionComputable, MesesComputables = b.MesesComputables,
        Monto = b.Monto, BonificacionExtraordinaria = b.BonificacionExtraordinaria, Observacion = b.Observacion,
        EstadoPago = b.EstadoPago, FechaPago = b.FechaPago, MedioPago = b.MedioPago
    };
}
