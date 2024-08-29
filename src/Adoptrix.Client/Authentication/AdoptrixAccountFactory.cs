using System.Security.Claims;
using System.Text.Json;
using Adoptrix.Client.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

namespace Adoptrix.Client.Authentication;

public class AdoptrixAccountFactory(IAccessTokenProviderAccessor accessor)
    : AccountClaimsPrincipalFactory<AdoptrixAccount>(accessor)
{
    private readonly IAccessTokenProvider tokenProvider = accessor.TokenProvider;

    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(AdoptrixAccount account,
        RemoteAuthenticationUserOptions options)
    {
        var principal = await base.CreateUserAsync(account, options);
        if (principal.Identity is null || !principal.Identity.IsAuthenticated ||
            principal.Identity is not ClaimsIdentity identity)
        {
            return principal;
        }

        // try and get the access token for the API
        var tokenResult = await tokenProvider.RequestAccessToken(new AccessTokenRequestOptions
        {
            Scopes = ["api://7e86487e-ac55-4988-8c1e-941d543cb376/access"]
        });
        if (!tokenResult.TryGetToken(out var token)) return principal;

        // get the roles from the access token and add them to the identity claims
        var claims = ParseTokenClaims(token);
        if (!claims.TryGetValue("roles", out var roles)) return principal;
        foreach (var role in roles.EnumerateArray())
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role.GetString()!));
        }

        return principal;
    }

    private static Dictionary<string, JsonElement> ParseTokenClaims(AccessToken token)
    {
        // JWTs come in the format: header.payload.signature
        var payload = token.Value.Split(".")[1]
            .DecodeBase64Url();

        var claims = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
            Convert.FromBase64String(payload)) ?? new Dictionary<string, JsonElement>();

        return claims;
    }
}
