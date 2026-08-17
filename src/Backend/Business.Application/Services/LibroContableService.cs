namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.DTOs.Contabilidad;
using Business.Application.Interfaces;
using Business.Domain.Common;
using Business.Domain.Entities.Contabilidad;
using Business.Domain.Interfaces;

public class LibroContableService : ILibroContableService
{
    private readonly IRepository<AsientoContable> _asientoRepo;
    private readonly IRepository<CuentaContable> _cuentaRepo;

    public LibroContableService(IRepository<AsientoContable> asientoRepo, IRepository<CuentaContable> cuentaRepo)
    {
        _asientoRepo = asientoRepo;
        _cuentaRepo = cuentaRepo;
    }

    public async Task<LibroMayorDto?> MayorAsync(string cuentaCodigo, DateTime desde, DateTime hasta)
    {
        var cuenta = (await _cuentaRepo.FindAsync(c => c.Codigo == cuentaCodigo)).FirstOrDefault();
        if (cuenta == null) return null;

        var asientos = await CargarAsientosAsync(desde, hasta);

        var lineas = asientos
            .SelectMany(a => a.Detalles.Where(d => d.CuentaContableId == cuenta.Id)
                .Select(d => new { a.Fecha, a.Numero, a.Glosa, d.Debe, d.Haber }))
            .OrderBy(x => x.Fecha).ThenBy(x => x.Numero)
            .ToList();

        var mayor = new LibroMayorDto
        {
            CuentaCodigo = cuenta.Codigo, CuentaNombre = cuenta.Nombre, Naturaleza = cuenta.Naturaleza,
            Desde = desde.Date, Hasta = hasta.Date
        };

        decimal saldo = 0;
        foreach (var l in lineas)
        {
            saldo += l.Debe - l.Haber;
            mayor.Movimientos.Add(new MayorLineaDto
            {
                Fecha = l.Fecha, AsientoNumero = l.Numero, Glosa = l.Glosa,
                Debe = l.Debe, Haber = l.Haber, SaldoAcumulado = saldo
            });
        }
        mayor.TotalDebe = lineas.Sum(x => x.Debe);
        mayor.TotalHaber = lineas.Sum(x => x.Haber);
        mayor.SaldoFinal = saldo;
        return mayor;
    }

    public async Task<BalanceComprobacionDto> BalanceComprobacionAsync(DateTime desde, DateTime hasta)
    {
        var asientos = await CargarAsientosAsync(desde, hasta);

        var cuentas = asientos
            .SelectMany(a => a.Detalles)
            .GroupBy(d => new { d.CuentaContable!.Codigo, d.CuentaContable.Nombre })
            .Select(g =>
            {
                var debe = g.Sum(x => x.Debe);
                var haber = g.Sum(x => x.Haber);
                var saldo = debe - haber;
                return new BalanceLineaDto
                {
                    Codigo = g.Key.Codigo, Nombre = g.Key.Nombre,
                    Debe = debe, Haber = haber,
                    SaldoDeudor = saldo > 0 ? saldo : 0,
                    SaldoAcreedor = saldo < 0 ? -saldo : 0
                };
            })
            .OrderBy(c => c.Codigo)
            .ToList();

        var balance = new BalanceComprobacionDto
        {
            Desde = desde.Date, Hasta = hasta.Date,
            Cuentas = cuentas,
            TotalDebe = cuentas.Sum(c => c.Debe),
            TotalHaber = cuentas.Sum(c => c.Haber),
            TotalSaldoDeudor = cuentas.Sum(c => c.SaldoDeudor),
            TotalSaldoAcreedor = cuentas.Sum(c => c.SaldoAcreedor)
        };
        balance.Cuadra = Math.Abs(balance.TotalDebe - balance.TotalHaber) < 0.01m
            && Math.Abs(balance.TotalSaldoDeudor - balance.TotalSaldoAcreedor) < 0.01m;
        return balance;
    }

    /// <summary>Carga los asientos registrados (no anulados) del período, con sus líneas y cuentas.</summary>
    private async Task<List<AsientoContable>> CargarAsientosAsync(DateTime desde, DateTime hasta)
    {
        var d = desde.Date;
        var h = hasta.Date.AddDays(1).AddTicks(-1);
        return await _asientoRepo.Query()
            .Include(a => a.Detalles).ThenInclude(x => x.CuentaContable)
            .Where(a => a.Fecha >= d && a.Fecha <= h && a.Estado == EstadoAsiento.Registrado)
            .ToListAsync();
    }
}
