namespace Business.Application.DTOs.Rrhh;

public class TrabajadorDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string TipoDocumento { get; set; } = string.Empty;
    public string NumeroDocumento { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string ApellidoPaterno { get; set; } = string.Empty;
    public string ApellidoMaterno { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public DateTime? FechaNacimiento { get; set; }
    public string? Sexo { get; set; }
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string Cargo { get; set; } = string.Empty;
    public DateTime FechaIngreso { get; set; }
    public DateTime? FechaCese { get; set; }
    public string TipoContrato { get; set; } = string.Empty;
    public decimal SueldoBasico { get; set; }
    public bool TieneAsignacionFamiliar { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string RegimenPension { get; set; } = string.Empty;
    public string? AfpNombre { get; set; }
    public string? Cuspp { get; set; }
}

public class CreateTrabajadorDto
{
    public string TipoDocumento { get; set; } = "DNI";
    public string NumeroDocumento { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string ApellidoPaterno { get; set; } = string.Empty;
    public string ApellidoMaterno { get; set; } = string.Empty;
    public DateTime? FechaNacimiento { get; set; }
    public string? Sexo { get; set; }
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string Cargo { get; set; } = string.Empty;
    public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;
    public string TipoContrato { get; set; } = "INDEFINIDO";
    public decimal SueldoBasico { get; set; }
    public bool TieneAsignacionFamiliar { get; set; }
    public string RegimenPension { get; set; } = "ONP";
    public string? AfpNombre { get; set; }
    public string? Cuspp { get; set; }
}
