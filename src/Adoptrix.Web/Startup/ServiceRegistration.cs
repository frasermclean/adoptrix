using Adoptrix.ServiceDefaults;
using Adoptrix.Web.Services;
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

        builder.Services.AddMudServices()
            .AddSingleton<AppNameProvider>()
            .AddSingleton<ThemeProvider>()
            .AddApiClients(builder.Configuration);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        return builder;
    }

    private static void AddApiClients(this IServiceCollection services, IConfiguration configuration)
    {
        const string apiClientName = "api";
        services.AddHttpClient(apiClientName, client => client.BaseAddress = new Uri("http://adoptrix-api"));
        services.AddHttpClient<IAnimalsClient, AnimalsClient>(apiClientName);
        services.AddHttpClient<IBreedsClient, BreedsClient>(apiClientName);
        services.AddHttpClient<ISpeciesClient, SpeciesClient>(apiClientName);
    }
}
