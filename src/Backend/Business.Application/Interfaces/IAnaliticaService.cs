namespace Business.Application.Interfaces;

using Business.Application.DTOs.Dashboard;

public interface IAnaliticaService
{
    /// <summary>Analítica gerencial de ventas en un rango de fechas (inclusive).</summary>
    Task<AnaliticaVentasDto> GetVentasAsync(DateTime desde, DateTime hasta);

    /// <summary>Analítica gerencial de inventario (valorización, rotación, stock crítico, sin movimiento).</summary>
    Task<AnaliticaInventarioDto> GetInventarioAsync(DateTime desde, DateTime hasta);

    /// <summary>Analítica gerencial financiera (aging de cartera CxC/CxP y estado de resultados con drill).</summary>
    Task<AnaliticaFinancieraDto> GetFinancieraAsync(DateTime desde, DateTime hasta);
}
