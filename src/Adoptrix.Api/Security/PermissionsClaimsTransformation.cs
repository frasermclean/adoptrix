using System.Security.Claims;
using Adoptrix.Api.Extensions;
using Adoptrix.Core;
using Microsoft.AspNetCore.Authentication;

namespace Adoptrix.Api.Security;

public class PermissionsClaimsTransformation(ILogger<PermissionsClaimsTransformation> logger) : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var roles = principal.GetRoles();
        var userId = principal.GetUserId();
        var claimsIdentity = principal.Identity as ClaimsIdentity;

        // add permissions
        var permissions = GetPermissions(roles);
        foreach (var permission in permissions)
        {
            claimsIdentity!.AddClaim(new Claim("permissions", permission));
        }

        logger.LogInformation(
            "User with ID {UserId} is in roles {Roles} and is granted {PermissionsCount} permissions",
            userId, roles, permissions.Length);

        return Task.FromResult(principal);
    }

    private static string[] GetPermissions(IEnumerable<UserRole> roles)
    {
        return roles.Contains(UserRole.Administrator) ? AdministratorPermissions : [];
    }

    private static readonly string[] AdministratorPermissions =
    [
        PermissionNames.AnimalsWrite,
        PermissionNames.BreedsWrite,
        PermissionNames.SpeciesWrite,
        PermissionNames.UsersManage
    ];
}
