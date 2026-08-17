namespace Business.Application.Interfaces;

/// <summary>
/// Provee a una empresa nueva de los catálogos base (plan de cuentas y conceptos de
/// planilla), clonándolos de la empresa base para que pueda operar de inmediato.
/// </summary>
public interface ICatalogoEmpresaService
{
    Task ClonarCatalogoBaseAsync(int empresaId, CancellationToken ct = default);
}
