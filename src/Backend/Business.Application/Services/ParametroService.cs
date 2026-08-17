namespace Business.Application.Services;

using System.Globalization;
using Business.Application.Interfaces;
using Business.Domain.Entities.Configuration;
using Business.Domain.Interfaces;

public class ParametroService : IParametroService
{
    private const decimal IgvPorDefecto = 0.18m;
    private const decimal RmvPorDefecto = 1025m;
    private const decimal TopeAseguradoAfpPorDefecto = 10878m;
    private readonly IRepository<Parametro> _repo;

    public ParametroService(IRepository<Parametro> repo) => _repo = repo;

    public async Task<decimal> GetIgvRateAsync(CancellationToken ct = default)
    {
        var valor = await LeerDecimalAsync("IGV");
        return valor.HasValue ? valor.Value / 100m : IgvPorDefecto;
    }

    public async Task<decimal> GetRmvAsync(CancellationToken ct = default)
        => await LeerDecimalAsync("RMV") ?? RmvPorDefecto;

    public async Task<decimal> GetTopeAseguradoAfpAsync(CancellationToken ct = default)
        => await LeerDecimalAsync("TOPE_ASEGURABLE_AFP") ?? TopeAseguradoAfpPorDefecto;

    private async Task<decimal?> LeerDecimalAsync(string codigo)
    {
        var parametro = (await _repo.FindAsync(p => p.Codigo == codigo)).FirstOrDefault();
        if (parametro != null && decimal.TryParse(parametro.Valor, NumberStyles.Any, CultureInfo.InvariantCulture, out var valor))
            return valor;
        return null;
    }
}
