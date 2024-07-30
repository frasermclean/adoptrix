using System.Security.Claims;
using Adoptrix.Api.Endpoints;
using Adoptrix.Api.Extensions;
using Adoptrix.Persistence.Services;
using Microsoft.Identity.Web;

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
        claimsIdentity!.AddClaim(new Claim(ClaimConstants.Roles, RoleNames.FromRole(user.Role)));
    }
}
