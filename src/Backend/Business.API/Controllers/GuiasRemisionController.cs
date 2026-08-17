namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Ventas;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/guias-remision")]
public class GuiasRemisionController : ControllerBase
{
    private readonly IGuiaRemisionService _service;
    public GuiasRemisionController(IGuiaRemisionService service) => _service = service;

    [HasPermission("sales.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<GuiaRemisionDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<GuiaRemisionDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("sales.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GuiaRemisionDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<GuiaRemisionDto>.Fail("Guía no encontrada")) : Ok(ApiResponse<GuiaRemisionDto>.Ok(result));
    }

    [HasPermission("sales.view")]
    [HttpGet("factura/{facturaId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<GuiaRemisionDto>>>> GetByFactura(int facturaId)
        => Ok(ApiResponse<IEnumerable<GuiaRemisionDto>>.Ok(await _service.GetByFacturaAsync(facturaId)));

    [HasPermission("sales.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<GuiaRemisionDto>>> Create([FromBody] CreateGuiaRemisionDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CreateAsync(dto, userName);
        return result == null
            ? NotFound(ApiResponse<GuiaRemisionDto>.Fail("Factura no encontrada"))
            : CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<GuiaRemisionDto>.Ok(result, "Guía de remisión emitida"));
    }
}
