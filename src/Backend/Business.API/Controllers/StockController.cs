namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Inventario;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;
    private readonly IMovimientoInventarioService _movimientoService;

    public StockController(IStockService stockService, IMovimientoInventarioService movimientoService)
    {
        _stockService = stockService;
        _movimientoService = movimientoService;
    }

    [HasPermission("inventory.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<StockDto>>>> GetAll()
        => Ok(ApiResponse<IEnumerable<StockDto>>.Ok(await _stockService.GetAllAsync()));

    [HasPermission("inventory.view")]
    [HttpGet("producto/{productoId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<StockDto>>>> GetByProducto(int productoId)
        => Ok(ApiResponse<IEnumerable<StockDto>>.Ok(await _stockService.GetByProductoAsync(productoId)));

    [HasPermission("inventory.view")]
    [HttpGet("almacen/{almacenId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<StockDto>>>> GetByAlmacen(int almacenId)
        => Ok(ApiResponse<IEnumerable<StockDto>>.Ok(await _stockService.GetByAlmacenAsync(almacenId)));

    [HasPermission("inventory.view")]
    [HttpGet("movimientos")]
    public async Task<ActionResult<ApiResponse<IEnumerable<MovimientoInventarioDto>>>> GetMovimientos()
        => Ok(ApiResponse<IEnumerable<MovimientoInventarioDto>>.Ok(await _movimientoService.GetAllAsync()));

    [HasPermission("inventory.manage")]
    [HttpPost("movimientos")]
    public async Task<ActionResult<ApiResponse<MovimientoInventarioDto>>> CreateMovimiento([FromBody] CreateMovimientoDto dto)
    {
        var userName = User.Identity?.Name ?? "sistema";
        var result = await _movimientoService.CreateAsync(dto, userName);
        return Ok(ApiResponse<MovimientoInventarioDto>.Ok(result, "Movimiento registrado exitosamente"));
    }
}
