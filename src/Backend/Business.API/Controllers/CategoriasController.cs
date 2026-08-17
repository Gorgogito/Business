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
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _service;
    public CategoriasController(ICategoriaService service) => _service = service;

    [HasPermission("masters.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoriaDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<CategoriaDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("masters.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CategoriaDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<CategoriaDto>.Fail("Categoría no encontrada")) : Ok(ApiResponse<CategoriaDto>.Ok(result));
    }

    [HasPermission("masters.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CategoriaDto>>> Create([FromBody] CreateCategoriaDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<CategoriaDto>.Ok(result));
    }

    [HasPermission("masters.manage")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CategoriaDto>>> Update(int id, [FromBody] CreateCategoriaDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result == null ? NotFound(ApiResponse<CategoriaDto>.Fail("Categoría no encontrada")) : Ok(ApiResponse<CategoriaDto>.Ok(result));
    }

    [HasPermission("masters.manage")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? Ok(ApiResponse<bool>.Ok(true)) : NotFound(ApiResponse<bool>.Fail("Categoría no encontrada"));
    }
}
