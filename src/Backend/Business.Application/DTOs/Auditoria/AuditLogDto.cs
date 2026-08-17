namespace Business.Application.DTOs.Auditoria;

public class AuditLogDto
{
    public int Id { get; set; }
    public string TableName { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? UserName { get; set; }
    public DateTime Timestamp { get; set; }
}
