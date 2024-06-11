using Adoptrix.Application.DependencyInjection;
using Adoptrix.Database.DependencyInjection;
using Adoptrix.Services;
using Adoptrix.Storage.DependencyInjection;
using MudBlazor.Services;

namespace Adoptrix.Startup;

public static class ServiceRegistration
{
    /// <summary>
    /// Registers services within the dependency injection container.
    /// </summary>
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddMudServices();

        builder.Services.AddApplicationServices();
        builder.Services.AddDatabaseServices(builder.Configuration);
        builder.Services.AddStorageServices(builder.Configuration);

        // local services
        builder.Services.AddSingleton<AppNameProvider>();
        builder.Services.AddSingleton<ThemeProvider>();

        return builder;
    }
}
