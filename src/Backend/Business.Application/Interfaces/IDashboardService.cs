namespace Business.Application.Interfaces;

using Business.Application.DTOs.Dashboard;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync();
}
