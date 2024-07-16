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
        var services = builder.Services;

        services.AddMudServices()
            .AddSingleton<AppNameProvider>()
            .AddSingleton<ThemeProvider>()
            .AddApiClients(builder.Configuration);

        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        return builder;
    }

    private static void AddApiClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("api", client =>
        {
            client.BaseAddress = new Uri(configuration["ApiEndpoint"]!);
        });
        services.AddHttpClient<IAnimalsClient, AnimalsClient>("api");
        services.AddHttpClient<IBreedsClient, BreedsClient>("api");
        services.AddHttpClient<ISpeciesClient, SpeciesClient>("api");
    }
}
