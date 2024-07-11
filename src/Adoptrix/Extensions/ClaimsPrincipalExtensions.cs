using System.Security.Claims;
using Microsoft.Identity.Web;

namespace Adoptrix.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserName(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimConstants.Name) ?? string.Empty;
    }

    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var claimValue = principal.FindFirstValue(ClaimConstants.ObjectId);

        return Guid.TryParse(claimValue, out var userId)
            ? userId
            : throw new InvalidDataException("Object ID claim is missing or invalid");
    }
}
