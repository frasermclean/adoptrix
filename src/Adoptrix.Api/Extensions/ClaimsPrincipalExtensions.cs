using System.Security.Claims;
using Adoptrix.Core;
using Microsoft.Identity.Web;

namespace Adoptrix.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static UserRole GetRole(this ClaimsPrincipal principal)
    {
        var claimValue = principal.FindFirstValue(ClaimConstants.Roles);
        return Enum.TryParse<UserRole>(claimValue, out var role)
            ? role
            : UserRole.User;
    }

    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var claimValue = principal.FindFirstValue(ClaimConstants.Oid);

        return Guid.TryParse(claimValue, out var userId)
            ? userId
            : throw new InvalidDataException("Object ID claim is missing or invalid");
    }
}
