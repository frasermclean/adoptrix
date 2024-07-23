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

        builder.AddMicrosoftIdentityPlatform();

        builder.Services.AddMudServices()
            .AddSingleton<AppNameProvider>()
            .AddSingleton<ThemeProvider>()
            .AddApiClients();

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        return builder;
    }

    private static WebApplicationBuilder AddMicrosoftIdentityPlatform(this WebApplicationBuilder builder)
    {
        // ensure claims are mapped correctly
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(builder.Configuration, "Authentication");

        builder.Services.AddControllersWithViews()
            .AddMicrosoftIdentityUI();

        return builder;
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
