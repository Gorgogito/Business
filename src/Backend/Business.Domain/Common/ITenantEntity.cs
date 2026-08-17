namespace Business.Domain.Common;

/// <summary>
/// Marca una entidad como perteneciente a una empresa (multiempresa). El sistema asigna
/// EmpresaId automáticamente al crear y filtra las consultas por la empresa del usuario.
/// </summary>
public interface ITenantEntity
{
    int EmpresaId { get; set; }
}
