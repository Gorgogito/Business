namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Contabilidad;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/contabilidad")]
public class LibrosContablesController : ControllerBase
{
    private readonly ILibroContableService _service;
    private readonly IEstadoFinancieroService _estados;
    public LibrosContablesController(ILibroContableService service, IEstadoFinancieroService estados)
    {
        _service = service;
        _estados = estados;
    }

    [HasPermission("accounting.view")]
    [HttpGet("mayor")]
    public async Task<ActionResult<ApiResponse<LibroMayorDto>>> Mayor([FromQuery] string cuenta, [FromQuery] DateTime desde, [FromQuery] DateTime hasta)
    {
        var result = await _service.MayorAsync(cuenta, desde, hasta);
        return result == null ? NotFound(ApiResponse<LibroMayorDto>.Fail("Cuenta no encontrada")) : Ok(ApiResponse<LibroMayorDto>.Ok(result));
    }

    [HasPermission("accounting.view")]
    [HttpGet("mayor/csv")]
    public async Task<IActionResult> MayorCsv([FromQuery] string cuenta, [FromQuery] DateTime desde, [FromQuery] DateTime hasta)
    {
        var result = await _service.MayorAsync(cuenta, desde, hasta);
        if (result == null) return NotFound();
        return File(CsvExport.ToCsv(result.Movimientos), "text/csv", $"mayor_{cuenta}_{desde:yyyyMMdd}_{hasta:yyyyMMdd}.csv");
    }

    [HasPermission("accounting.view")]
    [HttpGet("balance-comprobacion")]
    public async Task<ActionResult<ApiResponse<BalanceComprobacionDto>>> Balance([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        => Ok(ApiResponse<BalanceComprobacionDto>.Ok(await _service.BalanceComprobacionAsync(desde, hasta)));

    [HasPermission("accounting.view")]
    [HttpGet("balance-comprobacion/csv")]
    public async Task<IActionResult> BalanceCsv([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
    {
        var result = await _service.BalanceComprobacionAsync(desde, hasta);
        return File(CsvExport.ToCsv(result.Cuentas), "text/csv", $"balance_comprobacion_{desde:yyyyMMdd}_{hasta:yyyyMMdd}.csv");
    }

    [HasPermission("accounting.view")]
    [HttpGet("estado-resultados")]
    public async Task<ActionResult<ApiResponse<EstadoResultadosDto>>> EstadoResultados([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        => Ok(ApiResponse<EstadoResultadosDto>.Ok(await _estados.EstadoResultadosAsync(desde, hasta)));

    [HasPermission("accounting.view")]
    [HttpGet("balance-general")]
    public async Task<ActionResult<ApiResponse<BalanceGeneralDto>>> BalanceGeneral([FromQuery] DateTime hasta)
        => Ok(ApiResponse<BalanceGeneralDto>.Ok(await _estados.BalanceGeneralAsync(hasta)));
}
