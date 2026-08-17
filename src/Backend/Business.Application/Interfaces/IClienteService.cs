namespace Business.Application.Interfaces;

using Business.Application.DTOs.Maestros;

public interface IClienteService
{
    Task<IEnumerable<ClienteDto>> GetAllAsync();
    Task<ClienteDto?> GetByIdAsync(int id);
    Task<ClienteDto> CreateAsync(CreateClienteDto dto);
    Task<ClienteDto?> UpdateAsync(int id, CreateClienteDto dto);
    Task<bool> DeleteAsync(int id);
}
