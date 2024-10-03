using System.Security.Claims;
using Adoptrix.Api.Extensions;
using Adoptrix.Core;
using Microsoft.AspNetCore.Authentication;

namespace Adoptrix.Api.Security;

public class PermissionsClaimsTransformation(ILogger<PermissionsClaimsTransformation> logger) : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var role = principal.GetRole();
        var userId = principal.GetUserId();
        var claimsIdentity = principal.Identity as ClaimsIdentity;

        // add permissions
        var permissions = GetPermissions(role);
        foreach (var permission in permissions)
        {
            claimsIdentity!.AddClaim(new Claim("permissions", permission));
        }

        logger.LogInformation(
            "User with ID {UserId} is in role {RoleName} and is granted {PermissionsCount} permissions",
            userId, role, permissions.Length);

        return Task.FromResult(principal);
    }

    private static string[] GetPermissions(UserRole role) => role switch
    {
        UserRole.Administrator => AdministratorPermissions,
        _ => []
    };

    private static readonly string[] AdministratorPermissions =
    [
        PermissionNames.AnimalsWrite,
        PermissionNames.BreedsWrite,
        PermissionNames.SpeciesWrite,
        PermissionNames.UsersManage
    ];
}
