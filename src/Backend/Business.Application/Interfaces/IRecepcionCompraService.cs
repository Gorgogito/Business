namespace Business.Application.Interfaces;

using Business.Application.DTOs.Compras;

public interface IRecepcionCompraService
{
    Task<IEnumerable<RecepcionCompraDto>> GetAllAsync();
    Task<RecepcionCompraDto?> GetByIdAsync(int id);
    Task<RecepcionCompraDto> CreateAsync(CreateRecepcionCompraDto dto, string userName);
}
