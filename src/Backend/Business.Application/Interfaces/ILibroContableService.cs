namespace Business.Application.Interfaces;

using Business.Application.DTOs.Contabilidad;

public interface ILibroContableService
{
    /// <summary>Libro mayor de una cuenta: movimientos y saldo acumulado en el período.</summary>
    Task<LibroMayorDto?> MayorAsync(string cuentaCodigo, DateTime desde, DateTime hasta);

    /// <summary>Balance de comprobación: sumas y saldos por cuenta, con verificación de cuadre.</summary>
    Task<BalanceComprobacionDto> BalanceComprobacionAsync(DateTime desde, DateTime hasta);
}
