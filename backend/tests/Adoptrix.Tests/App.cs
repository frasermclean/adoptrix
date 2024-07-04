using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Tests;

public class App : AppFixture<Program>
{
    /// <summary>
    /// HTTP client pre-configured with test authentication.
    /// </summary>
    public HttpClient? TestAuthClient { get; private set; }

    protected override void ConfigureServices(IServiceCollection services)
    {
        // add test auth handler
        services.AddAuthentication(BasicAuthHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>(BasicAuthHandler.SchemeName, _ => { });
    }

    protected override Task SetupAsync()
    {
        TestAuthClient = CreateClient(options =>
            options.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(BasicAuthHandler.SchemeName));

        return Task.CompletedTask;
    }
}
