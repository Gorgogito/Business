namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Contabilidad;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/asientos-contables")]
public class AsientosContablesController : ControllerBase
{
    private readonly IAsientoContableService _service;
    public AsientosContablesController(IAsientoContableService service) => _service = service;

    [HasPermission("accounting.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<AsientoContableDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<AsientoContableDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("accounting.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<AsientoContableDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<AsientoContableDto>.Fail("Asiento no encontrado")) : Ok(ApiResponse<AsientoContableDto>.Ok(result));
    }

    [HasPermission("accounting.view")]
    [HttpGet("periodo")]
    public async Task<ActionResult<ApiResponse<IEnumerable<AsientoContableDto>>>> GetByPeriodo([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        => Ok(ApiResponse<IEnumerable<AsientoContableDto>>.Ok(await _service.GetByPeriodoAsync(desde, hasta)));

    [HasPermission("accounting.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<AsientoContableDto>>> Create([FromBody] CreateAsientoContableDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CreateAsync(dto, userName);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<AsientoContableDto>.Ok(result, "Asiento registrado"));
    }

    [HasPermission("accounting.manage")]
    [HttpPatch("{id}/anular")]
    public async Task<ActionResult<ApiResponse<AsientoContableDto>>> Anular(int id)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.AnularAsync(id, userName);
        return result == null ? NotFound(ApiResponse<AsientoContableDto>.Fail("Asiento no encontrado")) : Ok(ApiResponse<AsientoContableDto>.Ok(result, "Asiento anulado"));
    }
}
