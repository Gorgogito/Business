namespace Business.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Business.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business.Application.Common;
using Business.Application.DTOs.Auditoria;
using Business.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/auditoria")]
public class AuditoriaController : ControllerBase
{
    private readonly IAuditService _service;
    public AuditoriaController(IAuditService service) => _service = service;

    [HasPermission("security.view")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<AuditLogDto>>>> GetRecent([FromQuery] int take = 100)
        => Ok(ApiResponse<IEnumerable<AuditLogDto>>.Ok(await _service.GetRecentAsync(take)));

    [HasPermission("security.view")]
    [HttpGet("{tableName}/{entityId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<AuditLogDto>>>> GetByEntity(string tableName, string entityId)
        => Ok(ApiResponse<IEnumerable<AuditLogDto>>.Ok(await _service.GetByEntityAsync(tableName, entityId)));
}
