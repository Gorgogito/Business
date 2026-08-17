namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Rrhh;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/trabajadores")]
public class TrabajadoresController : ControllerBase
{
    private readonly ITrabajadorService _service;
    public TrabajadoresController(ITrabajadorService service) => _service = service;

    [HasPermission("hr.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<TrabajadorDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<TrabajadorDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("hr.view")]
    [HttpGet("activos")]
    public async Task<ActionResult<ApiResponse<IEnumerable<TrabajadorDto>>>> GetActivos()
        => Ok(ApiResponse<IEnumerable<TrabajadorDto>>.Ok(await _service.GetActivosAsync()));

    [HasPermission("hr.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<TrabajadorDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<TrabajadorDto>.Fail("Trabajador no encontrado")) : Ok(ApiResponse<TrabajadorDto>.Ok(result));
    }

    [HasPermission("hr.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<TrabajadorDto>>> Create([FromBody] CreateTrabajadorDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CreateAsync(dto, userName);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<TrabajadorDto>.Ok(result, "Trabajador registrado"));
    }

    [HasPermission("hr.manage")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<TrabajadorDto>>> Update(int id, [FromBody] CreateTrabajadorDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.UpdateAsync(id, dto, userName);
        return result == null ? NotFound(ApiResponse<TrabajadorDto>.Fail("Trabajador no encontrado")) : Ok(ApiResponse<TrabajadorDto>.Ok(result));
    }

    [HasPermission("hr.manage")]
    [HttpPatch("{id}/cesar")]
    public async Task<ActionResult<ApiResponse<TrabajadorDto>>> Cesar(int id, [FromBody] DateTime fechaCese)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CesarAsync(id, fechaCese, userName);
        return result == null ? NotFound(ApiResponse<TrabajadorDto>.Fail("Trabajador no encontrado")) : Ok(ApiResponse<TrabajadorDto>.Ok(result, "Trabajador cesado"));
    }

    [HasPermission("hr.manage")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? Ok(ApiResponse<bool>.Ok(true)) : NotFound(ApiResponse<bool>.Fail("Trabajador no encontrado"));
    }
}
