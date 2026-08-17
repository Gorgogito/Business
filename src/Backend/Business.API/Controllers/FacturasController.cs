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
public class FacturasController : ControllerBase
{
    private readonly IFacturaService _service;
    public FacturasController(IFacturaService service) => _service = service;

    [HasPermission("sales.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<FacturaDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<FacturaDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("sales.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<FacturaDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<FacturaDto>.Fail("Factura no encontrada")) : Ok(ApiResponse<FacturaDto>.Ok(result));
    }

    [HasPermission("sales.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<FacturaDto>>> Create([FromBody] CreateFacturaDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CreateAsync(dto, userName);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<FacturaDto>.Ok(result, "Factura emitida"));
    }

    [HasPermission("sales.manage")]
    [HttpPost("desde-pedido/{pedidoId}")]
    public async Task<ActionResult<ApiResponse<FacturaDto>>> CrearDesdePedido(int pedidoId, [FromQuery] string tipoDocumento = "FACTURA", [FromQuery] int almacenId = 1)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CrearDesdePedidoAsync(pedidoId, tipoDocumento, almacenId, userName);
        return result == null
            ? NotFound(ApiResponse<FacturaDto>.Fail("Pedido no encontrado"))
            : CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<FacturaDto>.Ok(result, "Factura generada desde pedido"));
    }

    [HasPermission("sales.manage")]
    [HttpPatch("{id}/estado")]
    public async Task<ActionResult<ApiResponse<FacturaDto>>> UpdateEstado(int id, [FromBody] string estado)
    {
        var result = await _service.UpdateEstadoAsync(id, estado);
        return result == null ? NotFound(ApiResponse<FacturaDto>.Fail("Factura no encontrada")) : Ok(ApiResponse<FacturaDto>.Ok(result));
    }
}
