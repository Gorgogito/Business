namespace Business.Application.Interfaces;

using Business.Application.DTOs.Auditoria;

public interface IAuditService
{
    Task<IEnumerable<AuditLogDto>> GetRecentAsync(int take = 100);
    Task<IEnumerable<AuditLogDto>> GetByEntityAsync(string tableName, string entityId);
}
