namespace Business.Application.Interfaces;

using Business.Application.DTOs.Finanzas;
using Business.Application.DTOs.Reportes;

public interface IReporteService
{
    Task<ReporteVentasDto> VentasPorPeriodoAsync(DateTime desde, DateTime hasta);
    Task<ReporteInventarioDto> ValorizacionInventarioAsync();
    Task<IEnumerable<CuentaPorCobrarDto>> CarteraPorCobrarAsync();
    Task<IEnumerable<CuentaPorPagarDto>> CarteraPorPagarAsync();
}
