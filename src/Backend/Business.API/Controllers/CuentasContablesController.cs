namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Contabilidad;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/cuentas-contables")]
public class CuentasContablesController : ControllerBase
{
    private readonly ICuentaContableService _service;
    public CuentasContablesController(ICuentaContableService service) => _service = service;

    [HasPermission("accounting.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CuentaContableDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<CuentaContableDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("accounting.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CuentaContableDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<CuentaContableDto>.Fail("Cuenta no encontrada")) : Ok(ApiResponse<CuentaContableDto>.Ok(result));
    }

    [HasPermission("accounting.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CuentaContableDto>>> Create([FromBody] CreateCuentaContableDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CreateAsync(dto, userName);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<CuentaContableDto>.Ok(result, "Cuenta creada"));
    }

    [HasPermission("accounting.manage")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CuentaContableDto>>> Update(int id, [FromBody] CreateCuentaContableDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.UpdateAsync(id, dto, userName);
        return result == null ? NotFound(ApiResponse<CuentaContableDto>.Fail("Cuenta no encontrada")) : Ok(ApiResponse<CuentaContableDto>.Ok(result));
    }

    [HasPermission("accounting.manage")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? Ok(ApiResponse<bool>.Ok(true)) : NotFound(ApiResponse<bool>.Fail("Cuenta no encontrada"));
    }
}
