namespace Business.API.Authorization;

using Microsoft.AspNetCore.Authorization;

/// <summary>Requisito de autorización que exige un código de permiso concreto.</summary>
public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }
    public PermissionRequirement(string permission) => Permission = permission;
}
