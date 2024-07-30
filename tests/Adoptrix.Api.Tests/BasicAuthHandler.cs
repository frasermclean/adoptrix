using System.Security.Claims;
using System.Text.Encodings.Web;
using Adoptrix.Api.Endpoints;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using ClaimTypes = Adoptrix.Api.Endpoints.ClaimTypes;

namespace Adoptrix.Api.Tests;

public class BasicAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "BasicAuthScheme";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Context.Request.Headers.Authorization != SchemeName)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid authorization header"));
        }

        var claims = new[]
        {
            new Claim(ClaimConstants.Name, "Bob Jones"),
            new Claim(ClaimConstants.ObjectId, Guid.NewGuid().ToString()),
            new Claim(ClaimConstants.Scope, "access"),
            new Claim(ClaimTypes.Permission, PermissionNames.AnimalsWrite),
            new Claim(ClaimTypes.Permission, PermissionNames.BreedsWrite),
            new Claim(ClaimTypes.Permission, PermissionNames.SpeciesWrite),

        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}
