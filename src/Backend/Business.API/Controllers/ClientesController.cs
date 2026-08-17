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
public class ClientesController : ControllerBase
{
    private readonly IClienteService _service;
    public ClientesController(IClienteService service) => _service = service;

    [HasPermission("masters.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ClienteDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<ClienteDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("masters.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ClienteDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<ClienteDto>.Fail("Cliente no encontrado")) : Ok(ApiResponse<ClienteDto>.Ok(result));
    }

    [HasPermission("masters.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ClienteDto>>> Create([FromBody] CreateClienteDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<ClienteDto>.Ok(result, "Cliente creado"));
    }

    [HasPermission("masters.manage")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ClienteDto>>> Update(int id, [FromBody] CreateClienteDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result == null ? NotFound(ApiResponse<ClienteDto>.Fail("Cliente no encontrado")) : Ok(ApiResponse<ClienteDto>.Ok(result));
    }

    [HasPermission("masters.manage")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? Ok(ApiResponse<bool>.Ok(true)) : NotFound(ApiResponse<bool>.Fail("Cliente no encontrado"));
    }
}
