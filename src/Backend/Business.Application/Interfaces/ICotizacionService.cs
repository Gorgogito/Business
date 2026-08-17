namespace Business.Application.Interfaces;

using Business.Application.DTOs.Ventas;

public interface ICotizacionService
{
    Task<IEnumerable<CotizacionDto>> GetAllAsync();
    Task<CotizacionDto?> GetByIdAsync(int id);
    Task<IEnumerable<CotizacionDto>> GetByClienteAsync(int clienteId);
    Task<CotizacionDto> CreateAsync(CreateCotizacionDto dto, string userName);
    Task<CotizacionDto?> UpdateEstadoAsync(int id, string estado);
    Task<bool> DeleteAsync(int id);
}
