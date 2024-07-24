using System.IdentityModel.Tokens.Jwt;
using Adoptrix.ServiceDefaults;
using Adoptrix.Web.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MudBlazor.Services;

namespace Adoptrix.Web.Startup;

public static class ServiceRegistration
{
    /// <summary>
    /// Registers services within the dependency injection container.
    /// </summary>
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.Services.AddMicrosoftIdentityPlatform(builder.Configuration)
            .AddMudServices()
            .AddSingleton<AppNameProvider>()
            .AddSingleton<ThemeProvider>()
            .AddApiClients();

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddMicrosoftIdentityConsentHandler();

        return builder;
    }

    private static IServiceCollection AddMicrosoftIdentityPlatform(this IServiceCollection services,
        IConfiguration configuration)
    {
        // ensure claims are mapped correctly
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

        var initialScopes = new[]
        {
            "api://7e86487e-ac55-4988-8c1e-941d543cb376/.default"
        };

        services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(configuration, "Authentication")
            .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
            .AddDownstreamApi("AdoptrixApi", configuration.GetSection("AdoptrixApi"))
            .AddInMemoryTokenCaches();

        services.AddControllersWithViews()
            .AddMicrosoftIdentityUI();

        return services;
    }

    private static void AddApiClients(this IServiceCollection services)
    {
        const string apiClientName = "api";
        services.AddHttpClient(apiClientName, client => client.BaseAddress = new Uri("http://adoptrix-api"));
        services.AddHttpClient<IAnimalsClient, AnimalsClient>(apiClientName);
        services.AddHttpClient<IBreedsClient, BreedsClient>(apiClientName);
        services.AddHttpClient<ISpeciesClient, SpeciesClient>(apiClientName);
    }
}
