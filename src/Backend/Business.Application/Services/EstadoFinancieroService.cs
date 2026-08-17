namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.DTOs.Contabilidad;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Contabilidad;
using Business.Domain.Interfaces;

public class EstadoFinancieroService : IEstadoFinancieroService
{
    private readonly IRepository<AsientoContable> _asientoRepo;

    public EstadoFinancieroService(IRepository<AsientoContable> asientoRepo) => _asientoRepo = asientoRepo;

    public async Task<EstadoResultadosDto> EstadoResultadosAsync(DateTime desde, DateTime hasta)
    {
        var saldos = await SaldosPorCuentaAsync(desde, hasta);

        // Ingresos (clase INGRESO, saldo acreedor) y costos/gastos (saldo deudor).
        var detalle = new List<EstadoLineaDto>();
        decimal ingresos = 0, costo = 0, gastos = 0;
        foreach (var s in saldos)
        {
            switch (s.Clase)
            {
                case ClaseCuenta.Ingreso:
                    var montoIng = -s.Saldo; // acreedor positivo
                    ingresos += montoIng;
                    detalle.Add(Linea("Ingresos", s, montoIng));
                    break;
                case ClaseCuenta.Costo:
                    costo += s.Saldo;
                    detalle.Add(Linea("Costo de ventas", s, s.Saldo));
                    break;
                case ClaseCuenta.Gasto:
                    gastos += s.Saldo;
                    detalle.Add(Linea("Gastos", s, s.Saldo));
                    break;
            }
        }

        var utilidadBruta = ingresos - costo;
        return new EstadoResultadosDto
        {
            Desde = desde.Date, Hasta = hasta.Date,
            Ingresos = ingresos, CostoVentas = costo, UtilidadBruta = utilidadBruta,
            Gastos = gastos, UtilidadNeta = utilidadBruta - gastos,
            Detalle = detalle
        };
    }

    public async Task<BalanceGeneralDto> BalanceGeneralAsync(DateTime hasta)
    {
        // Saldos acumulados desde el inicio de operaciones hasta la fecha de corte.
        var saldos = await SaldosPorCuentaAsync(DateTime.MinValue, hasta);

        var activos = new List<EstadoLineaDto>();
        var pasivos = new List<EstadoLineaDto>();
        var patrimonio = new List<EstadoLineaDto>();
        decimal utilidad = 0;

        foreach (var s in saldos)
        {
            switch (s.Clase)
            {
                case ClaseCuenta.Activo:
                    activos.Add(Linea("Activo", s, s.Saldo));
                    break;
                case ClaseCuenta.Pasivo:
                    pasivos.Add(Linea("Pasivo", s, -s.Saldo));
                    break;
                case ClaseCuenta.Patrimonio:
                    patrimonio.Add(Linea("Patrimonio", s, -s.Saldo));
                    break;
                case ClaseCuenta.Ingreso:
                    utilidad += -s.Saldo;      // ingresos suman a la utilidad
                    break;
                case ClaseCuenta.Costo:
                case ClaseCuenta.Gasto:
                    utilidad -= s.Saldo;       // costos y gastos restan
                    break;
            }
        }

        var totalActivo = activos.Sum(x => x.Monto);
        var totalPasivo = pasivos.Sum(x => x.Monto);
        var totalPatrimonio = patrimonio.Sum(x => x.Monto);
        var totalPasivoPatrimonio = totalPasivo + totalPatrimonio + utilidad;

        return new BalanceGeneralDto
        {
            Fecha = hasta.Date,
            Activos = activos, Pasivos = pasivos, Patrimonio = patrimonio,
            TotalActivo = totalActivo, TotalPasivo = totalPasivo, TotalPatrimonio = totalPatrimonio,
            UtilidadEjercicio = utilidad, TotalPasivoPatrimonio = totalPasivoPatrimonio,
            Cuadra = Math.Abs(totalActivo - totalPasivoPatrimonio) < 0.01m
        };
    }

    private static EstadoLineaDto Linea(string grupo, CuentaSaldo s, decimal monto) =>
        new() { Grupo = grupo, Codigo = s.Codigo, Nombre = s.Nombre, Monto = monto };

    /// <summary>Calcula el saldo (debe − haber) por cuenta a partir de los asientos registrados.</summary>
    private async Task<List<CuentaSaldo>> SaldosPorCuentaAsync(DateTime desde, DateTime hasta)
    {
        var d = desde == DateTime.MinValue ? DateTime.MinValue : desde.Date;
        var h = hasta.Date.AddDays(1).AddTicks(-1);

        var asientos = await _asientoRepo.Query()
            .Include(a => a.Detalles).ThenInclude(x => x.CuentaContable)
            .Where(a => a.Fecha >= d && a.Fecha <= h && a.Estado == EstadoAsiento.Registrado)
            .ToListAsync();

        return asientos
            .SelectMany(a => a.Detalles)
            .GroupBy(x => new { x.CuentaContable!.Codigo, x.CuentaContable.Nombre, x.CuentaContable.Clase })
            .Select(g => new CuentaSaldo
            {
                Codigo = g.Key.Codigo, Nombre = g.Key.Nombre, Clase = g.Key.Clase,
                Saldo = g.Sum(x => x.Debe) - g.Sum(x => x.Haber)
            })
            .OrderBy(x => x.Codigo)
            .ToList();
    }

    private sealed class CuentaSaldo
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Clase { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
    }
}
