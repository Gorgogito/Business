namespace Business.Domain.Entities.Rrhh;

using Business.Domain.Common;

/// <summary>
/// Legajo del trabajador: datos personales, condición laboral y afiliación previsional.
/// Base para el cálculo de planillas y boletas de pago.
/// </summary>
public class Trabajador : BaseEntity, ITenantEntity
{
    /// <summary>Empresa propietaria del registro (multiempresa).</summary>
    public int EmpresaId { get; set; } = 1;

    public string Codigo { get; set; } = string.Empty;

    // Datos personales
    public string TipoDocumento { get; set; } = "DNI"; // DNI, CE, PASAPORTE
    public string NumeroDocumento { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string ApellidoPaterno { get; set; } = string.Empty;
    public string ApellidoMaterno { get; set; } = string.Empty;
    public DateTime? FechaNacimiento { get; set; }
    public string? Sexo { get; set; } // M, F
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }

    // Datos laborales
    public string Cargo { get; set; } = string.Empty;
    public DateTime FechaIngreso { get; set; }
    public DateTime? FechaCese { get; set; }
    public string TipoContrato { get; set; } = "INDEFINIDO";
    public decimal SueldoBasico { get; set; }
    /// <summary>Si percibe asignación familiar (10% de la RMV) por tener hijos.</summary>
    public bool TieneAsignacionFamiliar { get; set; }
    public string Estado { get; set; } = "ACTIVO"; // ACTIVO, CESADO

    // Régimen previsional
    public string RegimenPension { get; set; } = "ONP"; // AFP, ONP
    public string? AfpNombre { get; set; }               // Integra, Prima, Profuturo, Habitat
    public string? Cuspp { get; set; }                   // código único de afiliación (AFP)
}
