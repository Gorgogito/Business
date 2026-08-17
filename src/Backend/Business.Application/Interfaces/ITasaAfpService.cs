namespace Business.Application.Interfaces;

using Business.Application.DTOs.Rrhh;

public interface ITasaAfpService
{
    Task<IEnumerable<TasaAfpDto>> GetAllAsync();
    Task<TasaAfpDto?> GetByIdAsync(int id);
    Task<TasaAfpDto> CreateAsync(CreateTasaAfpDto dto, string userName);
    Task<TasaAfpDto?> UpdateAsync(int id, CreateTasaAfpDto dto, string userName);
    Task<bool> DeleteAsync(int id);
}
