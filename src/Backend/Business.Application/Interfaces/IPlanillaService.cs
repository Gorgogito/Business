namespace Business.Application.Interfaces;

using Business.Application.DTOs.Rrhh;

public interface IPlanillaService
{
    Task<IEnumerable<PlanillaDto>> GetAllAsync();
    Task<PlanillaDto?> GetByIdAsync(int id);

    /// <summary>
    /// Procesa la planilla de un período: calcula la boleta de cada trabajador activo
    /// (ingresos, descuentos según régimen previsional, aporte de EsSalud y neto).
    /// </summary>
    Task<PlanillaDto> ProcesarAsync(int anio, int mes, string userName);

    Task<PlanillaDto?> AnularAsync(int id, string userName);

    /// <summary>Boleta de pago de un trabajador dentro de una planilla.</summary>
    Task<PlanillaBoletaDto?> GetBoletaAsync(int planillaId, int trabajadorId);

    /// <summary>Historial de boletas de un trabajador (todas las planillas no anuladas).</summary>
    Task<IEnumerable<PlanillaBoletaDto>> GetBoletasPorTrabajadorAsync(int trabajadorId);
}
