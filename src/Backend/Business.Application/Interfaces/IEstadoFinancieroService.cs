namespace Business.Application.Interfaces;

using Business.Application.DTOs.Contabilidad;

public interface IEstadoFinancieroService
{
    /// <summary>Estado de resultados del período: ingresos − costo de ventas − gastos = utilidad.</summary>
    Task<EstadoResultadosDto> EstadoResultadosAsync(DateTime desde, DateTime hasta);

    /// <summary>Balance general a una fecha de corte: activo = pasivo + patrimonio + utilidad del ejercicio.</summary>
    Task<BalanceGeneralDto> BalanceGeneralAsync(DateTime hasta);
}
