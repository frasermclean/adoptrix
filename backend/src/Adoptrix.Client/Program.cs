using Adoptrix.Client.Handlers.Animals;
using Adoptrix.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

namespace Adoptrix.Client;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = WebAssemblyHostBuilder.CreateDefault(args)
            .RegisterServices()
            .Build();

        await host.RunAsync();
    }

    private static WebAssemblyHostBuilder RegisterServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services
            .AddCommonServices()
            .AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
            .AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssemblyContaining<GetAnimalQueryHandler>();
            });

        return builder;
    }

    /// <summary>
    /// Adds services that are common to both the server and client projects.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddMudServices()
            .AddSingleton<AppNameProvider>()
            .AddSingleton<ThemeProvider>();

        return services;
    }
}
