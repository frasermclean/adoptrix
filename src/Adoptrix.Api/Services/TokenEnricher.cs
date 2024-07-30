using System.Security.Claims;
using Adoptrix.Api.Endpoints;
using Adoptrix.Api.Extensions;
using Adoptrix.Core;
using Adoptrix.Persistence.Services;
using Microsoft.Identity.Web;
using ClaimTypes = Adoptrix.Api.Endpoints.ClaimTypes;

namespace Adoptrix.Api.Services;

public interface ITokenEnricher
{
    Task EnrichAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default);
}

public class TokenEnricher(ILogger<TokenEnricher> logger, IUsersRepository usersRepository) : ITokenEnricher
{
    public async Task EnrichAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
    {
        var userId = claimsPrincipal.GetUserId();

        var user = await usersRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            logger.LogError("User with id {UserId} not found, cannot enrich token", userId);
            return;
        }

        var claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
        claimsIdentity!.AddClaim(new Claim(ClaimConstants.Roles, GetRoleName(user.Role)));

        // add permissions
        var permissions = GetPermissions(user.Role);
        foreach (var permission in permissions)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Permission, permission));
        }
    }

    private static string GetRoleName(UserRole role) => role switch
    {
        UserRole.Administrator => RoleNames.Administrator,
        _ => RoleNames.User
    };

    private static readonly string[] AdministratorPermissions =
    [
        PermissionNames.AnimalsWrite,
        PermissionNames.BreedsWrite,
        PermissionNames.SpeciesWrite
    ];

    private static string[] GetPermissions(UserRole role) => role switch
    {
        UserRole.Administrator => AdministratorPermissions,
        _ => []
    };
}
