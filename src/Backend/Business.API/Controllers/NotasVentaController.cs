namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Ventas;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/notas-venta")]
public class NotasVentaController : ControllerBase
{
    private readonly INotaVentaService _service;
    public NotasVentaController(INotaVentaService service) => _service = service;

    [HasPermission("sales.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<NotaVentaDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<NotaVentaDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("sales.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<NotaVentaDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<NotaVentaDto>.Fail("Nota no encontrada")) : Ok(ApiResponse<NotaVentaDto>.Ok(result));
    }

    [HasPermission("sales.view")]
    [HttpGet("factura/{facturaId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<NotaVentaDto>>>> GetByFactura(int facturaId)
        => Ok(ApiResponse<IEnumerable<NotaVentaDto>>.Ok(await _service.GetByFacturaAsync(facturaId)));

    [HasPermission("sales.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<NotaVentaDto>>> Create([FromBody] CreateNotaVentaDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CreateAsync(dto, userName);
        return result == null
            ? NotFound(ApiResponse<NotaVentaDto>.Fail("Factura no encontrada"))
            : CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<NotaVentaDto>.Ok(result, "Nota emitida"));
    }
}
