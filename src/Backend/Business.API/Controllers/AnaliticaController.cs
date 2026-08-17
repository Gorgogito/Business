namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Dashboard;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/analitica")]
public class AnaliticaController : ControllerBase
{
    private readonly IAnaliticaService _service;
    public AnaliticaController(IAnaliticaService service) => _service = service;

    [HasPermission("sales.view")]
    [HttpGet("ventas")]
    public async Task<ActionResult<ApiResponse<AnaliticaVentasDto>>> GetVentas(
        [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
    {
        // Por defecto: últimos 6 meses hasta hoy.
        var hoy = DateTime.UtcNow.Date;
        var hastaReal = hasta ?? hoy;
        var desdeReal = desde ?? hastaReal.AddMonths(-6);
        var result = await _service.GetVentasAsync(desdeReal, hastaReal);
        return Ok(ApiResponse<AnaliticaVentasDto>.Ok(result));
    }

    [HasPermission("inventory.view")]
    [HttpGet("inventario")]
    public async Task<ActionResult<ApiResponse<AnaliticaInventarioDto>>> GetInventario(
        [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
    {
        // Por defecto: últimos 6 meses hasta hoy (para rotación y sin movimiento).
        var hoy = DateTime.UtcNow.Date;
        var hastaReal = hasta ?? hoy;
        var desdeReal = desde ?? hastaReal.AddMonths(-6);
        var result = await _service.GetInventarioAsync(desdeReal, hastaReal);
        return Ok(ApiResponse<AnaliticaInventarioDto>.Ok(result));
    }

    [HasPermission("accounting.view")]
    [HttpGet("financiera")]
    public async Task<ActionResult<ApiResponse<AnaliticaFinancieraDto>>> GetFinanciera(
        [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
    {
        // Por defecto: año en curso hasta hoy (aging al corte, EE.RR. del período).
        var hoy = DateTime.UtcNow.Date;
        var hastaReal = hasta ?? hoy;
        var desdeReal = desde ?? new DateTime(hastaReal.Year, 1, 1);
        var result = await _service.GetFinancieraAsync(desdeReal, hastaReal);
        return Ok(ApiResponse<AnaliticaFinancieraDto>.Ok(result));
    }
}
