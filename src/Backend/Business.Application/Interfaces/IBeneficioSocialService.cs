namespace Business.Application.Interfaces;

using Business.Application.DTOs.Rrhh;

public interface IBeneficioSocialService
{
    Task<IEnumerable<BeneficioSocialDto>> GetAllAsync();
    Task<IEnumerable<BeneficioSocialDto>> GetByTrabajadorAsync(int trabajadorId);

    /// <summary>Calcula y registra la CTS del semestre para todos los trabajadores activos.
    /// semestre 1 = mayo–octubre (depósito noviembre); semestre 2 = noviembre–abril (depósito mayo del año siguiente).</summary>
    Task<IEnumerable<BeneficioSocialDto>> CalcularCtsAsync(int anio, int semestre, string userName);

    /// <summary>Calcula y registra la gratificación del semestre para todos los trabajadores activos.
    /// semestre 1 = enero–junio (Fiestas Patrias); semestre 2 = julio–diciembre (Navidad). Incluye bonificación extraordinaria 9%.</summary>
    Task<IEnumerable<BeneficioSocialDto>> CalcularGratificacionAsync(int anio, int semestre, string userName);

    /// <summary>Calcula y registra la remuneración vacacional anual (récord) para todos los trabajadores activos.</summary>
    Task<IEnumerable<BeneficioSocialDto>> CalcularVacacionesAsync(int anio, string userName);

    /// <summary>
    /// Registra el pago de un beneficio ya calculado: genera el asiento de pago (413 beneficios
    /// sociales por pagar debe / caja-bancos haber) y marca el registro como PAGADO.
    /// </summary>
    Task<BeneficioSocialDto?> RegistrarPagoAsync(int id, string medioPago, string userName);
}
