namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Security;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _service;

    public RolesController(IRoleService service) => _service = service;

    [HasPermission("security.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<RoleDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<RoleDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("security.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<RoleDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<RoleDto>.Fail("Rol no encontrado")) : Ok(ApiResponse<RoleDto>.Ok(result));
    }

    [HasPermission("security.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<RoleDto>>> Create([FromBody] CreateRoleDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<RoleDto>.Ok(result, "Rol creado exitosamente"));
    }

    [HasPermission("security.manage")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<RoleDto>>> Update(int id, [FromBody] CreateRoleDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result == null ? NotFound(ApiResponse<RoleDto>.Fail("Rol no encontrado")) : Ok(ApiResponse<RoleDto>.Ok(result));
    }

    [HasPermission("security.manage")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? Ok(ApiResponse<bool>.Ok(true)) : NotFound(ApiResponse<bool>.Fail("Rol no encontrado"));
    }
}
