using Adoptrix.Application.DependencyInjection;
using Adoptrix.Client;
using Adoptrix.Database.DependencyInjection;
using Adoptrix.Storage.DependencyInjection;

namespace Adoptrix.Startup;

public static class ServiceRegistration
{
    /// <summary>
    /// Registers services within the dependency injection container.
    /// </summary>
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.AddCommonServices()
            .AddApplicationServices()
            .AddDatabaseServices(builder.Configuration)
            .AddStorageServices(builder.Configuration);

        return builder;
    }
}
