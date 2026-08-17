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
public class ProductosController : ControllerBase
{
    private readonly IProductoService _service;
    public ProductosController(IProductoService service) => _service = service;

    [HasPermission("masters.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductoDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<ProductoDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("masters.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProductoDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<ProductoDto>.Fail("Producto no encontrado")) : Ok(ApiResponse<ProductoDto>.Ok(result));
    }

    [HasPermission("masters.view")]
    [HttpGet("categoria/{categoriaId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductoDto>>>> GetByCategoria(int categoriaId)
        => Ok(ApiResponse<IEnumerable<ProductoDto>>.Ok(await _service.GetByCategoriaAsync(categoriaId)));

    [HasPermission("masters.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProductoDto>>> Create([FromBody] CreateProductoDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<ProductoDto>.Ok(result, "Producto creado"));
    }

    [HasPermission("masters.manage")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ProductoDto>>> Update(int id, [FromBody] CreateProductoDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result == null ? NotFound(ApiResponse<ProductoDto>.Fail("Producto no encontrado")) : Ok(ApiResponse<ProductoDto>.Ok(result));
    }

    [HasPermission("masters.manage")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? Ok(ApiResponse<bool>.Ok(true)) : NotFound(ApiResponse<bool>.Fail("Producto no encontrado"));
    }
}
