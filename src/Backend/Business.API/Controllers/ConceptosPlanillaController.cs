namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Rrhh;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/conceptos-planilla")]
public class ConceptosPlanillaController : ControllerBase
{
    private readonly IConceptoPlanillaService _service;
    public ConceptosPlanillaController(IConceptoPlanillaService service) => _service = service;

    [HasPermission("hr.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ConceptoPlanillaDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<ConceptoPlanillaDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("hr.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ConceptoPlanillaDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<ConceptoPlanillaDto>.Fail("Concepto no encontrado")) : Ok(ApiResponse<ConceptoPlanillaDto>.Ok(result));
    }

    [HasPermission("hr.manage")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ConceptoPlanillaDto>>> Create([FromBody] CreateConceptoPlanillaDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CreateAsync(dto, userName);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<ConceptoPlanillaDto>.Ok(result, "Concepto creado"));
    }

    [HasPermission("hr.manage")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ConceptoPlanillaDto>>> Update(int id, [FromBody] CreateConceptoPlanillaDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.UpdateAsync(id, dto, userName);
        return result == null ? NotFound(ApiResponse<ConceptoPlanillaDto>.Fail("Concepto no encontrado")) : Ok(ApiResponse<ConceptoPlanillaDto>.Ok(result));
    }

    [HasPermission("hr.manage")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? Ok(ApiResponse<bool>.Ok(true)) : NotFound(ApiResponse<bool>.Fail("Concepto no encontrado"));
    }
}
