namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Maestros;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProveedoresController : ControllerBase
{
    private readonly IProveedorService _service;
    public ProveedoresController(IProveedorService service) => _service = service;

    [HasPermission("masters.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProveedorDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<ProveedorDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("masters.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProveedorDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<ProveedorDto>.Fail("Proveedor no encontrado")) : Ok(ApiResponse<ProveedorDto>.Ok(result));
    }

    [HasPermission("masters.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProveedorDto>>> Create([FromBody] CreateProveedorDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<ProveedorDto>.Ok(result, "Proveedor creado"));
    }

    [HasPermission("masters.manage")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ProveedorDto>>> Update(int id, [FromBody] CreateProveedorDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result == null ? NotFound(ApiResponse<ProveedorDto>.Fail("Proveedor no encontrado")) : Ok(ApiResponse<ProveedorDto>.Ok(result));
    }

    [HasPermission("masters.manage")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? Ok(ApiResponse<bool>.Ok(true)) : NotFound(ApiResponse<bool>.Fail("Proveedor no encontrado"));
    }
}
