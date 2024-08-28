using System.Security.Claims;
using System.Text.Encodings.Web;
using Adoptrix.Logic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;

namespace Adoptrix.Api.Tests.Fixtures;

public class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "TestAuth";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorizationHeader = Context.Request.Headers.Authorization.FirstOrDefault() ?? string.Empty;
        if (!authorizationHeader.StartsWith(SchemeName))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid authorization header"));
        }

        var parts = authorizationHeader.Split('-');
        var role = parts.Length > 1 ? parts[1] : RoleNames.User;

        var claims = new[]
        {
            new Claim(ClaimConstants.Name, "Bob Jones"),
            new Claim(ClaimConstants.Oid, Guid.NewGuid().ToString()),
            new Claim(ClaimConstants.Scope, "access"),
            new Claim(ClaimConstants.Roles, role)
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}
