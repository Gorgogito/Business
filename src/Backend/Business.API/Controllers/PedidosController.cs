namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Ventas;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _service;
    public PedidosController(IPedidoService service) => _service = service;

    [HasPermission("sales.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<PedidoDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<PedidoDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("sales.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PedidoDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<PedidoDto>.Fail("Pedido no encontrado")) : Ok(ApiResponse<PedidoDto>.Ok(result));
    }

    [HasPermission("sales.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<PedidoDto>>> Create([FromBody] CreatePedidoDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CreateAsync(dto, userName);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<PedidoDto>.Ok(result, "Pedido creado"));
    }

    [HasPermission("sales.manage")]
    [HttpPost("desde-cotizacion/{cotizacionId}")]
    public async Task<ActionResult<ApiResponse<PedidoDto>>> CrearDesdeCotizacion(int cotizacionId)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CrearDesdeCotizacionAsync(cotizacionId, userName);
        return result == null
            ? NotFound(ApiResponse<PedidoDto>.Fail("Cotización no encontrada"))
            : CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<PedidoDto>.Ok(result, "Pedido generado desde cotización"));
    }

    [HasPermission("sales.manage")]
    [HttpPatch("{id}/estado")]
    public async Task<ActionResult<ApiResponse<PedidoDto>>> UpdateEstado(int id, [FromBody] string estado)
    {
        var result = await _service.UpdateEstadoAsync(id, estado);
        return result == null ? NotFound(ApiResponse<PedidoDto>.Fail("Pedido no encontrado")) : Ok(ApiResponse<PedidoDto>.Ok(result));
    }

    [HasPermission("sales.manage")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? Ok(ApiResponse<bool>.Ok(true)) : NotFound(ApiResponse<bool>.Fail("Pedido no encontrado"));
    }
}
