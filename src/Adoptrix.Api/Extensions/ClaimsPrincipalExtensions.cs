﻿using System.Security.Claims;
using Microsoft.Identity.Web;

namespace Adoptrix.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetRole(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimConstants.Roles) ?? string.Empty;
    }

    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var claimValue = principal.FindFirstValue(ClaimConstants.Oid);

        return Guid.TryParse(claimValue, out var userId)
            ? userId
            : throw new InvalidDataException("Object ID claim is missing or invalid");
    }
}
