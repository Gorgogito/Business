namespace Business.Domain.Entities.Rrhh;

using Business.Domain.Common;

/// <summary>
/// Beneficio social calculado para un trabajador en un período: CTS (compensación por tiempo de
/// servicios), gratificación (Fiestas Patrias / Navidad) o remuneración vacacional. Registra la
/// remuneración computable, los meses computables y el monto resultante.
/// </summary>
public class BeneficioSocial : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public int TrabajadorId { get; set; }
    public Trabajador? Trabajador { get; set; }

    /// <summary>CTS, GRATIFICACION o VACACIONES.</summary>
    public string Tipo { get; set; } = string.Empty;
    /// <summary>Período que cubre el beneficio (ej. "2026-1", "2026-JUL").</summary>
    public string Periodo { get; set; } = string.Empty;
    public DateTime FechaCalculo { get; set; }

    /// <summary>Remuneración computable base del cálculo.</summary>
    public decimal RemuneracionComputable { get; set; }
    /// <summary>Meses computables considerados (según fecha de ingreso).</summary>
    public decimal MesesComputables { get; set; }
    /// <summary>Monto del beneficio (sin la bonificación extraordinaria).</summary>
    public decimal Monto { get; set; }
    /// <summary>Bonificación extraordinaria 9% (Ley 30334) para gratificaciones.</summary>
    public decimal BonificacionExtraordinaria { get; set; }
    public string? Observacion { get; set; }

    /// <summary>PENDIENTE o PAGADO. Al pagar se genera el asiento de pago (413 debe / caja-bancos haber).</summary>
    public string EstadoPago { get; set; } = EstadoPagoBeneficio.Pendiente;
    public DateTime? FechaPago { get; set; }
    public string? MedioPago { get; set; }
}
