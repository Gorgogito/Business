namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Rrhh;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/beneficios-sociales")]
public class BeneficiosSocialesController : ControllerBase
{
    private readonly IBeneficioSocialService _service;
    public BeneficiosSocialesController(IBeneficioSocialService service) => _service = service;

    [HasPermission("hr.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<BeneficioSocialDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<BeneficioSocialDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("hr.view")]
    [HttpGet("trabajador/{trabajadorId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BeneficioSocialDto>>>> GetByTrabajador(int trabajadorId)
        => Ok(ApiResponse<IEnumerable<BeneficioSocialDto>>.Ok(await _service.GetByTrabajadorAsync(trabajadorId)));

    [HasPermission("hr.manage")]
    [HttpPost("cts")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BeneficioSocialDto>>>> CalcularCts([FromQuery] int anio, [FromQuery] int semestre)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CalcularCtsAsync(anio, semestre, userName);
        return Ok(ApiResponse<IEnumerable<BeneficioSocialDto>>.Ok(result, "CTS calculada"));
    }

    [HasPermission("hr.manage")]
    [HttpPost("gratificacion")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BeneficioSocialDto>>>> CalcularGratificacion([FromQuery] int anio, [FromQuery] int semestre)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CalcularGratificacionAsync(anio, semestre, userName);
        return Ok(ApiResponse<IEnumerable<BeneficioSocialDto>>.Ok(result, "Gratificación calculada"));
    }

    [HasPermission("hr.manage")]
    [HttpPost("vacaciones")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BeneficioSocialDto>>>> CalcularVacaciones([FromQuery] int anio)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.CalcularVacacionesAsync(anio, userName);
        return Ok(ApiResponse<IEnumerable<BeneficioSocialDto>>.Ok(result, "Vacaciones calculadas"));
    }

    [HasPermission("hr.manage")]
    [HttpPatch("{id}/pagar")]
    public async Task<ActionResult<ApiResponse<BeneficioSocialDto>>> Pagar(int id, [FromBody] RegistrarPagoBeneficioDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.RegistrarPagoAsync(id, dto.MedioPago, userName);
        return result == null ? NotFound(ApiResponse<BeneficioSocialDto>.Fail("Beneficio no encontrado")) : Ok(ApiResponse<BeneficioSocialDto>.Ok(result, "Pago registrado"));
    }
}
