using System.Security.Claims;
using Adoptrix.Core;
using Microsoft.Identity.Web;

namespace Adoptrix.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static IEnumerable<UserRole> GetRoles(this ClaimsPrincipal principal)
    {
        return principal.FindAll(ClaimConstants.Roles)
            .Select(claim => Enum.Parse<UserRole>(claim.Value));
    }

    public static Guid? GetUserId(this ClaimsPrincipal principal)
    {
        var objectId = principal.GetObjectId();

        return objectId is null ? null : Guid.Parse(objectId);
    }
}
