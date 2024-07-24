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

        services.AddMudServices();
        services.AddSingleton<ThemeProvider>();
        services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        return builder;
    }
}
