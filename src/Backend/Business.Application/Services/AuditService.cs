namespace Business.Application.Services;

using Microsoft.EntityFrameworkCore;
using Business.Application.DTOs.Auditoria;
using Business.Application.Interfaces;
using Business.Domain.Entities.Auditoria;
using Business.Domain.Interfaces;

public class AuditService : IAuditService
{
    private readonly IRepository<AuditLog> _repo;

    public AuditService(IRepository<AuditLog> repo) => _repo = repo;

    public async Task<IEnumerable<AuditLogDto>> GetRecentAsync(int take = 100)
    {
        var items = await _repo.Query()
            .OrderByDescending(a => a.Timestamp)
            .Take(take <= 0 ? 100 : take)
            .ToListAsync();
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<AuditLogDto>> GetByEntityAsync(string tableName, string entityId)
    {
        var items = await _repo.Query()
            .Where(a => a.TableName == tableName && a.EntityId == entityId)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
        return items.Select(MapToDto);
    }

    private static AuditLogDto MapToDto(AuditLog a) => new()
    {
        Id = a.Id, TableName = a.TableName, EntityId = a.EntityId, Action = a.Action,
        OldValues = a.OldValues, NewValues = a.NewValues, UserName = a.UserName, Timestamp = a.Timestamp
    };
}
