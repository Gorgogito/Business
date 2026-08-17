namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Compras;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/ordenes-compra")]
public class OrdenesCompraController : ControllerBase
{
    private readonly IOrdenCompraService _service;
    public OrdenesCompraController(IOrdenCompraService service) => _service = service;

    [HasPermission("purchases.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<OrdenCompraDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<OrdenCompraDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("purchases.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<OrdenCompraDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<OrdenCompraDto>.Fail("Orden de compra no encontrada")) : Ok(ApiResponse<OrdenCompraDto>.Ok(result));
    }

    [HasPermission("purchases.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<OrdenCompraDto>>> Create([FromBody] CreateOrdenCompraDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CreateAsync(dto, userName);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<OrdenCompraDto>.Ok(result, "Orden de compra creada"));
    }

    [HasPermission("purchases.manage")]
    [HttpPatch("{id}/estado")]
    public async Task<ActionResult<ApiResponse<OrdenCompraDto>>> UpdateEstado(int id, [FromBody] string estado)
    {
        var result = await _service.UpdateEstadoAsync(id, estado);
        return result == null ? NotFound(ApiResponse<OrdenCompraDto>.Fail("OC no encontrada")) : Ok(ApiResponse<OrdenCompraDto>.Ok(result));
    }

    [HasPermission("purchases.manage")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? Ok(ApiResponse<bool>.Ok(true)) : NotFound(ApiResponse<bool>.Fail("OC no encontrada"));
    }
}
