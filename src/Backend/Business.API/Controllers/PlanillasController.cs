namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Rrhh;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/planillas")]
public class PlanillasController : ControllerBase
{
    private readonly IPlanillaService _service;
    public PlanillasController(IPlanillaService service) => _service = service;

    [HasPermission("hr.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<PlanillaDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<PlanillaDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("hr.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PlanillaDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<PlanillaDto>.Fail("Planilla no encontrada")) : Ok(ApiResponse<PlanillaDto>.Ok(result));
    }

    [HasPermission("hr.manage")]
    [HttpPost("procesar")]
    public async Task<ActionResult<ApiResponse<PlanillaDto>>> Procesar([FromBody] ProcesarPlanillaDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.ProcesarAsync(dto.Anio, dto.Mes, userName);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<PlanillaDto>.Ok(result, "Planilla procesada"));
    }

    [HasPermission("hr.manage")]
    [HttpPatch("{id}/anular")]
    public async Task<ActionResult<ApiResponse<PlanillaDto>>> Anular(int id)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.AnularAsync(id, userName);
        return result == null ? NotFound(ApiResponse<PlanillaDto>.Fail("Planilla no encontrada")) : Ok(ApiResponse<PlanillaDto>.Ok(result, "Planilla anulada"));
    }

    [HasPermission("hr.view")]
    [HttpGet("{id}/boleta/{trabajadorId}")]
    public async Task<ActionResult<ApiResponse<PlanillaBoletaDto>>> Boleta(int id, int trabajadorId)
    {
        var result = await _service.GetBoletaAsync(id, trabajadorId);
        return result == null ? NotFound(ApiResponse<PlanillaBoletaDto>.Fail("Boleta no encontrada")) : Ok(ApiResponse<PlanillaBoletaDto>.Ok(result));
    }

    [HasPermission("hr.view")]
    [HttpGet("{id}/boleta/{trabajadorId}/csv")]
    public async Task<IActionResult> BoletaCsv(int id, int trabajadorId)
    {
        var boleta = await _service.GetBoletaAsync(id, trabajadorId);
        if (boleta == null) return NotFound();
        return File(CsvExport.ToCsv(boleta.Conceptos), "text/csv", $"boleta_{boleta.TrabajadorCodigo}_{id}.csv");
    }

    [HasPermission("hr.view")]
    [HttpGet("boletas/trabajador/{trabajadorId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<PlanillaBoletaDto>>>> BoletasTrabajador(int trabajadorId)
        => Ok(ApiResponse<IEnumerable<PlanillaBoletaDto>>.Ok(await _service.GetBoletasPorTrabajadorAsync(trabajadorId)));
}
