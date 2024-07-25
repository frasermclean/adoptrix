using Adoptrix.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

namespace Adoptrix.Client;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var app = WebAssemblyHostBuilder.CreateDefault(args)
            .AddRootComponents()
            .AddServices()
            .Build();

        await app.RunAsync();
    }

    private static WebAssemblyHostBuilder AddRootComponents(this WebAssemblyHostBuilder builder)
    {
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        return builder;
    }

    private static WebAssemblyHostBuilder AddServices(this WebAssemblyHostBuilder builder)
    {
        var services = builder.Services;

        services.AddMsalAuthentication(options =>
        {
            builder.Configuration.Bind("Authentication", options.ProviderOptions.Authentication);
            options.ProviderOptions.Cache.CacheLocation = "localStorage";
            options.ProviderOptions.DefaultAccessTokenScopes.Add("https://graph.microsoft.com/user.read");
        });

        services.AddMudServices()
            .AddGraphApiClient(builder.Configuration)
            .AddApiClients(builder.Configuration["AdoptrixApi:BaseUrl"]!)
            .AddSingleton<ThemeProvider>()
            .AddSingleton(new ImageUrlResolver(builder.Configuration["BlobStorage:AnimalsImagesBaseUrl"]!));

        return builder;
    }

    private static IServiceCollection AddGraphApiClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<GraphApiAuthorizationMessageHandler>();
        services.AddHttpClient<IGraphApiClient, GraphApiClient>(client =>
                client.BaseAddress = new Uri(configuration["MicrosoftGraph:BaseUrl"]!))
            .AddHttpMessageHandler<GraphApiAuthorizationMessageHandler>();

        return services;
    }

    private static IServiceCollection AddApiClients(this IServiceCollection services, string baseUrl)
    {
        const string apiClientName = "ApiClient";

        services.AddHttpClient(apiClientName, client => client.BaseAddress = new Uri(baseUrl));
        services.AddHttpClient<IAnimalsClient, AnimalsClient>(apiClientName);
        services.AddHttpClient<IBreedsClient, BreedsClient>(apiClientName);
        services.AddHttpClient<ISpeciesClient, SpeciesClient>(apiClientName);

        return services;
    }
}
