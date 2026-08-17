namespace Business.API.Authorization;

using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Exige que el usuario tenga el permiso indicado. Uso: [HasPermission("sales.manage")].
/// </summary>
public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
        => Policy = $"{PermissionPolicyProvider.Prefix}{permission}";
}
