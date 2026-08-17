namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Produccion;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/recetas")]
public class RecetasController : ControllerBase
{
    private readonly IRecetaService _service;
    public RecetasController(IRecetaService service) => _service = service;

    [HasPermission("production.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<RecetaDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<RecetaDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("production.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<RecetaDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<RecetaDto>.Fail("Receta no encontrada")) : Ok(ApiResponse<RecetaDto>.Ok(result));
    }

    [HasPermission("production.view")]
    [HttpGet("producto/{productoId}")]
    public async Task<ActionResult<ApiResponse<RecetaDto>>> GetByProducto(int productoId)
    {
        var result = await _service.GetByProductoAsync(productoId);
        return result == null ? NotFound(ApiResponse<RecetaDto>.Fail("El producto no tiene receta activa")) : Ok(ApiResponse<RecetaDto>.Ok(result));
    }

    [HasPermission("production.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<RecetaDto>>> Create([FromBody] CreateRecetaDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CreateAsync(dto, userName);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<RecetaDto>.Ok(result, "Receta creada"));
    }

    [HasPermission("production.manage")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<RecetaDto>>> Update(int id, [FromBody] CreateRecetaDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.UpdateAsync(id, dto, userName);
        return result == null ? NotFound(ApiResponse<RecetaDto>.Fail("Receta no encontrada")) : Ok(ApiResponse<RecetaDto>.Ok(result));
    }

    [HasPermission("production.manage")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? Ok(ApiResponse<bool>.Ok(true)) : NotFound(ApiResponse<bool>.Fail("Receta no encontrada"));
    }
}
