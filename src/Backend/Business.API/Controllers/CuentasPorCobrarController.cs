namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Finanzas;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/cuentas-por-cobrar")]
public class CuentasPorCobrarController : ControllerBase
{
    private readonly ICuentaPorCobrarService _service;
    public CuentasPorCobrarController(ICuentaPorCobrarService service) => _service = service;

    [HasPermission("sales.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CuentaPorCobrarDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<CuentaPorCobrarDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("sales.view")]
    [HttpGet("pendientes")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CuentaPorCobrarDto>>>> GetPendientes()
        => Ok(ApiResponse<IEnumerable<CuentaPorCobrarDto>>.Ok(await _service.GetPendientesAsync()));

    [HasPermission("sales.view")]
    [HttpGet("cliente/{clienteId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CuentaPorCobrarDto>>>> GetByCliente(int clienteId)
        => Ok(ApiResponse<IEnumerable<CuentaPorCobrarDto>>.Ok(await _service.GetByClienteAsync(clienteId)));

    [HasPermission("sales.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CuentaPorCobrarDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<CuentaPorCobrarDto>.Fail("Cuenta por cobrar no encontrada")) : Ok(ApiResponse<CuentaPorCobrarDto>.Ok(result));
    }

    [HasPermission("sales.manage")]
    [HttpPost("{id}/cobros")]
    public async Task<ActionResult<ApiResponse<CobroDto>>> RegistrarCobro(int id, [FromBody] CreateCobroDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.RegistrarCobroAsync(id, dto, userName);
        return result == null
            ? NotFound(ApiResponse<CobroDto>.Fail("Cuenta por cobrar no encontrada"))
            : Ok(ApiResponse<CobroDto>.Ok(result, "Cobro registrado"));
    }
}
