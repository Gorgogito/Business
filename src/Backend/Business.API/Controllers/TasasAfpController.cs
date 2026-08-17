namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Rrhh;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/tasas-afp")]
public class TasasAfpController : ControllerBase
{
    private readonly ITasaAfpService _service;
    public TasasAfpController(ITasaAfpService service) => _service = service;

    [HasPermission("hr.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<TasaAfpDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<TasaAfpDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("hr.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<TasaAfpDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<TasaAfpDto>.Fail("AFP no encontrada")) : Ok(ApiResponse<TasaAfpDto>.Ok(result));
    }

    [HasPermission("hr.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<TasaAfpDto>>> Create([FromBody] CreateTasaAfpDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CreateAsync(dto, userName);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<TasaAfpDto>.Ok(result, "AFP creada"));
    }

    [HasPermission("hr.manage")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<TasaAfpDto>>> Update(int id, [FromBody] CreateTasaAfpDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.UpdateAsync(id, dto, userName);
        return result == null ? NotFound(ApiResponse<TasaAfpDto>.Fail("AFP no encontrada")) : Ok(ApiResponse<TasaAfpDto>.Ok(result));
    }

    [HasPermission("hr.manage")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? Ok(ApiResponse<bool>.Ok(true)) : NotFound(ApiResponse<bool>.Fail("AFP no encontrada"));
    }
}
