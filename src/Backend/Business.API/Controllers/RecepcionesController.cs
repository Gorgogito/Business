namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Compras;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RecepcionesController : ControllerBase
{
    private readonly IRecepcionCompraService _service;
    public RecepcionesController(IRecepcionCompraService service) => _service = service;

    [HasPermission("purchases.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<RecepcionCompraDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<RecepcionCompraDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("purchases.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<RecepcionCompraDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<RecepcionCompraDto>.Fail("Recepción no encontrada")) : Ok(ApiResponse<RecepcionCompraDto>.Ok(result));
    }

    [HasPermission("purchases.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<RecepcionCompraDto>>> Create([FromBody] CreateRecepcionCompraDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CreateAsync(dto, userName);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<RecepcionCompraDto>.Ok(result, "Recepción registrada"));
    }
}
