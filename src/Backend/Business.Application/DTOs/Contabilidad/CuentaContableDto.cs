namespace Business.Application.DTOs.Contabilidad;

public class CuentaContableDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Clase { get; set; } = string.Empty;
    public string Naturaleza { get; set; } = string.Empty;
    public int Nivel { get; set; }
    public bool EsMovimiento { get; set; }
}

public class CreateCuentaContableDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Clase { get; set; } = string.Empty; // ACTIVO, PASIVO, PATRIMONIO, INGRESO, GASTO, COSTO
    public bool EsMovimiento { get; set; } = true;
}
