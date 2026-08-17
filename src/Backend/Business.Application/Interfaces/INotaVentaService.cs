namespace Business.Application.Interfaces;

using Business.Application.DTOs.Ventas;

public interface INotaVentaService
{
    Task<IEnumerable<NotaVentaDto>> GetAllAsync();
    Task<NotaVentaDto?> GetByIdAsync(int id);
    Task<IEnumerable<NotaVentaDto>> GetByFacturaAsync(int facturaId);

    /// <summary>
    /// Crea una nota de crédito o débito ligada a una factura. La de crédito reingresa
    /// stock y reduce la cuenta por cobrar; la de débito la incrementa.
    /// </summary>
    Task<NotaVentaDto?> CreateAsync(CreateNotaVentaDto dto, string userName);
}
