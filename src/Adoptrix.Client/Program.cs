using Adoptrix.Client.Services;
using Microsoft.AspNetCore.Components;
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
        builder.Services.AddCommonServices();

        return builder;
    }

    /// <summary>
    /// Adds services that are common to both the server and client projects.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services
            .AddScoped(serviceProvider =>
            {
                var navigationManager = serviceProvider.GetRequiredService<NavigationManager>();
                return new HttpClient { BaseAddress = new Uri(navigationManager.BaseUri) };
            })
            .AddScoped<IAnimalsClient, AnimalsClient>()
            .AddScoped<IBreedsClient, BreedsClient>()
            .AddScoped<ISpeciesClient, SpeciesClient>()
            .AddMudServices()
            .AddSingleton<AppNameProvider>()
            .AddSingleton<ThemeProvider>();

        return services;
    }
}
