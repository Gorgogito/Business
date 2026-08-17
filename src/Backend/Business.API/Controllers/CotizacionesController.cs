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
public class CotizacionesController : ControllerBase
{
    private readonly ICotizacionService _service;
    public CotizacionesController(ICotizacionService service) => _service = service;

    [HasPermission("sales.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CotizacionDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<CotizacionDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("sales.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CotizacionDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<CotizacionDto>.Fail("Cotización no encontrada")) : Ok(ApiResponse<CotizacionDto>.Ok(result));
    }

    [HasPermission("sales.view")]
    [HttpGet("cliente/{clienteId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CotizacionDto>>>> GetByCliente(int clienteId)
        => Ok(ApiResponse<IEnumerable<CotizacionDto>>.Ok(await _service.GetByClienteAsync(clienteId)));

    [HasPermission("sales.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CotizacionDto>>> Create([FromBody] CreateCotizacionDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CreateAsync(dto, userName);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<CotizacionDto>.Ok(result, "Cotización creada"));
    }

    [HasPermission("sales.manage")]
    [HttpPatch("{id}/estado")]
    public async Task<ActionResult<ApiResponse<CotizacionDto>>> UpdateEstado(int id, [FromBody] string estado)
    {
        var result = await _service.UpdateEstadoAsync(id, estado);
        return result == null ? NotFound(ApiResponse<CotizacionDto>.Fail("Cotización no encontrada")) : Ok(ApiResponse<CotizacionDto>.Ok(result));
    }

    [HasPermission("sales.manage")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? Ok(ApiResponse<bool>.Ok(true)) : NotFound(ApiResponse<bool>.Fail("Cotización no encontrada"));
    }
}
