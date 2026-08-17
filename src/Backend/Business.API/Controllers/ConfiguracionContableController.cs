namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Contabilidad;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/configuracion-contable")]
public class ConfiguracionContableController : ControllerBase
{
    private readonly IConfiguracionContableService _service;
    public ConfiguracionContableController(IConfiguracionContableService service) => _service = service;

    [HasPermission("accounting.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ConfiguracionCuentaContableDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<ConfiguracionCuentaContableDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("accounting.manage")]
    [HttpPut("{concepto}")]
    public async Task<ActionResult<ApiResponse<ConfiguracionCuentaContableDto>>> Configurar(string concepto, [FromBody] ConfigurarCuentaContableDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.ConfigurarAsync(concepto, dto.CuentaContableId, userName);
        return Ok(ApiResponse<ConfiguracionCuentaContableDto>.Ok(result, "Cuenta configurada"));
    }
}
