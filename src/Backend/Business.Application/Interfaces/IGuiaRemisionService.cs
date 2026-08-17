namespace Business.Application.Interfaces;

using Business.Application.DTOs.Ventas;

public interface IGuiaRemisionService
{
    Task<IEnumerable<GuiaRemisionDto>> GetAllAsync();
    Task<GuiaRemisionDto?> GetByIdAsync(int id);
    Task<IEnumerable<GuiaRemisionDto>> GetByFacturaAsync(int facturaId);

    /// <summary>
    /// Crea una guía de remisión. Si se indica una factura, toma su cliente y, cuando no
    /// se envían detalles, arrastra los ítems de la factura.
    /// </summary>
    Task<GuiaRemisionDto?> CreateAsync(CreateGuiaRemisionDto dto, string userName);
}
