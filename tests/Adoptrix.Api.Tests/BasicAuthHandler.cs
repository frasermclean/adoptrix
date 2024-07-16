using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;

namespace Adoptrix.Api.Tests;

public class BasicAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "BasicAuthScheme";
    public const string UserName = "Bob Jones";
    public static readonly Guid UserId = Guid.NewGuid();

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Context.Request.Headers.Authorization != SchemeName)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid authorization header"));
        }

        var claims = new[]
        {
            new Claim(ClaimConstants.Name, UserName),
            new Claim(ClaimConstants.ObjectId, UserId.ToString()),
            new Claim(ClaimConstants.Scope, "access")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}
