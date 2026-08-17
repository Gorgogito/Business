namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Configuration;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SucursalesController : ControllerBase
{
    private readonly ISucursalService _service;
    public SucursalesController(ISucursalService service) => _service = service;

    [HasPermission("config.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<SucursalDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<SucursalDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("config.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<SucursalDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<SucursalDto>.Fail("Sucursal no encontrada")) : Ok(ApiResponse<SucursalDto>.Ok(result));
    }

    [HasPermission("config.view")]
    [HttpGet("empresa/{empresaId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<SucursalDto>>>> GetByEmpresa(int empresaId)
        => Ok(ApiResponse<IEnumerable<SucursalDto>>.Ok(await _service.GetByEmpresaAsync(empresaId)));

    [HasPermission("config.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<SucursalDto>>> Create([FromBody] CreateSucursalDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<SucursalDto>.Ok(result));
    }

    [HasPermission("config.manage")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<SucursalDto>>> Update(int id, [FromBody] CreateSucursalDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result == null ? NotFound(ApiResponse<SucursalDto>.Fail("Sucursal no encontrada")) : Ok(ApiResponse<SucursalDto>.Ok(result));
    }

    [HasPermission("config.manage")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? Ok(ApiResponse<bool>.Ok(true)) : NotFound(ApiResponse<bool>.Fail("Sucursal no encontrada"));
    }
}
