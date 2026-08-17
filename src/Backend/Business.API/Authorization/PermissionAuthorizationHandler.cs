namespace Business.API.Authorization;

using Microsoft.AspNetCore.Authorization;

/// <summary>Concede acceso si el usuario posee el claim "permission" con el código requerido.</summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.HasClaim("permission", requirement.Permission))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
