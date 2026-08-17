namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Produccion;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/ordenes-fabricacion")]
public class OrdenesFabricacionController : ControllerBase
{
    private readonly IOrdenFabricacionService _service;
    public OrdenesFabricacionController(IOrdenFabricacionService service) => _service = service;

    [HasPermission("production.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<OrdenFabricacionDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<OrdenFabricacionDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("production.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<OrdenFabricacionDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<OrdenFabricacionDto>.Fail("Orden no encontrada")) : Ok(ApiResponse<OrdenFabricacionDto>.Ok(result));
    }

    [HasPermission("production.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<OrdenFabricacionDto>>> Create([FromBody] CreateOrdenFabricacionDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CreateAsync(dto, userName);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<OrdenFabricacionDto>.Ok(result, "Orden de fabricación creada"));
    }

    [HasPermission("production.manage")]
    [HttpPost("{id}/procesar")]
    public async Task<ActionResult<ApiResponse<OrdenFabricacionDto>>> Procesar(int id, [FromBody] ProcesarOrdenFabricacionDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.ProcesarAsync(id, dto, userName);
        return result == null ? NotFound(ApiResponse<OrdenFabricacionDto>.Fail("Orden no encontrada")) : Ok(ApiResponse<OrdenFabricacionDto>.Ok(result, "Orden procesada"));
    }

    [HasPermission("production.manage")]
    [HttpPatch("{id}/anular")]
    public async Task<ActionResult<ApiResponse<OrdenFabricacionDto>>> Anular(int id)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.AnularAsync(id, userName);
        return result == null ? NotFound(ApiResponse<OrdenFabricacionDto>.Fail("Orden no encontrada")) : Ok(ApiResponse<OrdenFabricacionDto>.Ok(result, "Orden anulada"));
    }
}
