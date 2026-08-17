namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Inventario;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AlmacenesController : ControllerBase
{
    private readonly IAlmacenService _service;
    public AlmacenesController(IAlmacenService service) => _service = service;

    [HasPermission("inventory.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<AlmacenDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<AlmacenDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("inventory.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<AlmacenDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<AlmacenDto>.Fail("Almacén no encontrado")) : Ok(ApiResponse<AlmacenDto>.Ok(result));
    }

    [HasPermission("inventory.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<AlmacenDto>>> Create([FromBody] CreateAlmacenDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<AlmacenDto>.Ok(result));
    }

    [HasPermission("inventory.manage")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<AlmacenDto>>> Update(int id, [FromBody] CreateAlmacenDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result == null ? NotFound(ApiResponse<AlmacenDto>.Fail("Almacén no encontrado")) : Ok(ApiResponse<AlmacenDto>.Ok(result));
    }

    [HasPermission("inventory.manage")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? Ok(ApiResponse<bool>.Ok(true)) : NotFound(ApiResponse<bool>.Fail("Almacén no encontrado"));
    }
}
