namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Configuration;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmpresasController : ControllerBase
{
    private readonly IEmpresaService _service;
    public EmpresasController(IEmpresaService service) => _service = service;

    [HasPermission("config.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<EmpresaDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<EmpresaDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("config.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<EmpresaDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<EmpresaDto>.Fail("Empresa no encontrada")) : Ok(ApiResponse<EmpresaDto>.Ok(result));
    }

    [HasPermission("config.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<EmpresaDto>>> Create([FromBody] CreateEmpresaDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<EmpresaDto>.Ok(result, "Empresa creada"));
    }

    [HasPermission("config.manage")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<EmpresaDto>>> Update(int id, [FromBody] CreateEmpresaDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result == null ? NotFound(ApiResponse<EmpresaDto>.Fail("Empresa no encontrada")) : Ok(ApiResponse<EmpresaDto>.Ok(result));
    }

    [HasPermission("config.manage")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? Ok(ApiResponse<bool>.Ok(true)) : NotFound(ApiResponse<bool>.Fail("Empresa no encontrada"));
    }

    [HasPermission("config.manage")]
    [HttpPost("{id}/aprovisionar-catalogo")]
    public async Task<ActionResult<ApiResponse<bool>>> AprovisionarCatalogo(int id)
    {
        var result = await _service.AprovisionarCatalogoAsync(id);
        return result ? Ok(ApiResponse<bool>.Ok(true, "Catálogo aprovisionado")) : NotFound(ApiResponse<bool>.Fail("Empresa no encontrada"));
    }
}
