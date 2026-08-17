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
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service) => _service = service;

    [HasPermission("security.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<UserDto>>.Ok(result));
    }

    [HasPermission("security.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound(ApiResponse<UserDto>.Fail("Usuario no encontrado"));
        return Ok(ApiResponse<UserDto>.Ok(result));
    }

    [HasPermission("security.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<UserDto>>> Create([FromBody] CreateUserDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<UserDto>.Ok(result, "Usuario creado exitosamente"));
    }

    [HasPermission("security.manage")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Update(int id, [FromBody] UpdateUserDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null) return NotFound(ApiResponse<UserDto>.Fail("Usuario no encontrado"));
        return Ok(ApiResponse<UserDto>.Ok(result, "Usuario actualizado exitosamente"));
    }

    [HasPermission("security.manage")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result) return NotFound(ApiResponse<bool>.Fail("Usuario no encontrado"));
        return Ok(ApiResponse<bool>.Ok(true, "Usuario eliminado exitosamente"));
    }
}
