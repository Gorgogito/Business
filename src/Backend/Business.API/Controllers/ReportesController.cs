namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Reportes;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/reportes")]
public class ReportesController : ControllerBase
{
    private readonly IReporteService _service;
    public ReportesController(IReporteService service) => _service = service;

    // ---- Ventas por período ----

    [HasPermission("sales.view")]
    [HttpGet("ventas")]
    public async Task<ActionResult<ApiResponse<ReporteVentasDto>>> Ventas([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        => Ok(ApiResponse<ReporteVentasDto>.Ok(await _service.VentasPorPeriodoAsync(desde, hasta)));

    [HasPermission("sales.view")]
    [HttpGet("ventas/csv")]
    public async Task<IActionResult> VentasCsv([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
    {
        var reporte = await _service.VentasPorPeriodoAsync(desde, hasta);
        return File(CsvExport.ToCsv(reporte.Detalle), "text/csv", $"ventas_{desde:yyyyMMdd}_{hasta:yyyyMMdd}.csv");
    }

    // ---- Valorización de inventario ----

    [HasPermission("inventory.view")]
    [HttpGet("inventario")]
    public async Task<ActionResult<ApiResponse<ReporteInventarioDto>>> Inventario()
        => Ok(ApiResponse<ReporteInventarioDto>.Ok(await _service.ValorizacionInventarioAsync()));

    [HasPermission("inventory.view")]
    [HttpGet("inventario/csv")]
    public async Task<IActionResult> InventarioCsv()
    {
        var reporte = await _service.ValorizacionInventarioAsync();
        return File(CsvExport.ToCsv(reporte.Items), "text/csv", "valorizacion_inventario.csv");
    }

    // ---- Cartera por cobrar ----

    [HasPermission("sales.view")]
    [HttpGet("cartera-cobrar")]
    public async Task<IActionResult> CarteraCobrar()
        => Ok(ApiResponse<object>.Ok(await _service.CarteraPorCobrarAsync()));

    [HasPermission("sales.view")]
    [HttpGet("cartera-cobrar/csv")]
    public async Task<IActionResult> CarteraCobrarCsv()
        => File(CsvExport.ToCsv(await _service.CarteraPorCobrarAsync()), "text/csv", "cartera_por_cobrar.csv");

    // ---- Cartera por pagar ----

    [HasPermission("purchases.view")]
    [HttpGet("cartera-pagar")]
    public async Task<IActionResult> CarteraPagar()
        => Ok(ApiResponse<object>.Ok(await _service.CarteraPorPagarAsync()));

    [HasPermission("purchases.view")]
    [HttpGet("cartera-pagar/csv")]
    public async Task<IActionResult> CarteraPagarCsv()
        => File(CsvExport.ToCsv(await _service.CarteraPorPagarAsync()), "text/csv", "cartera_por_pagar.csv");
}
