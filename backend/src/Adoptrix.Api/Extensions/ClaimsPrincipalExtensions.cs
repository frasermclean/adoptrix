﻿using System.Security.Claims;
using Microsoft.Identity.Web;

namespace Adoptrix.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var claimValue = principal.FindFirstValue(ClaimConstants.NameIdentifierId);

        return Guid.TryParse(claimValue, out var userId)
            ? userId
            : throw new InvalidDataException("User ID claim is missing or invalid");
    }
}
