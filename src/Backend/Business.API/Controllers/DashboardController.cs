namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Dashboard;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;
    public DashboardController(IDashboardService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<DashboardDto>>> Get()
        => Ok(ApiResponse<DashboardDto>.Ok(await _service.GetDashboardAsync()));
}
