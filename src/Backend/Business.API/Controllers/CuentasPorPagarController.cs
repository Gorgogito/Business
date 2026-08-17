namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Finanzas;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/cuentas-por-pagar")]
public class CuentasPorPagarController : ControllerBase
{
    private readonly ICuentaPorPagarService _service;
    public CuentasPorPagarController(ICuentaPorPagarService service) => _service = service;

    [HasPermission("purchases.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CuentaPorPagarDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<CuentaPorPagarDto>>.Ok(await _service.GetAllAsync()));

    [HasPermission("purchases.view")]
    [HttpGet("pendientes")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CuentaPorPagarDto>>>> GetPendientes()
        => Ok(ApiResponse<IEnumerable<CuentaPorPagarDto>>.Ok(await _service.GetPendientesAsync()));

    [HasPermission("purchases.view")]
    [HttpGet("proveedor/{proveedorId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CuentaPorPagarDto>>>> GetByProveedor(int proveedorId)
        => Ok(ApiResponse<IEnumerable<CuentaPorPagarDto>>.Ok(await _service.GetByProveedorAsync(proveedorId)));

    [HasPermission("purchases.view")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CuentaPorPagarDto>>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound(ApiResponse<CuentaPorPagarDto>.Fail("Cuenta por pagar no encontrada")) : Ok(ApiResponse<CuentaPorPagarDto>.Ok(result));
    }

    [HasPermission("purchases.manage")]
    [HttpPost("{id}/pagos")]
    public async Task<ActionResult<ApiResponse<PagoDto>>> RegistrarPago(int id, [FromBody] CreatePagoDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _service.RegistrarPagoAsync(id, dto, userName);
        return result == null
            ? NotFound(ApiResponse<PagoDto>.Fail("Cuenta por pagar no encontrada"))
            : Ok(ApiResponse<PagoDto>.Ok(result, "Pago registrado"));
    }
}
