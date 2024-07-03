using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Client.Services;
using Adoptrix.Domain.Services;
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
            .AddScoped<IAnimalsService, AnimalsClient>()
            .AddScoped<IBreedsService, BreedsClient>()
            .AddScoped<ISpeciesService, SpeciesClient>()
            .AddSingleton(new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
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
