using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Application.DependencyInjection;
using Adoptrix.Client;
using Adoptrix.Database.DependencyInjection;
using Adoptrix.Storage.DependencyInjection;
using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using FastEndpoints;
using Microsoft.AspNetCore.Http.Json;

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

        builder.Services.AddFastEndpoints();

        builder.Services.AddCommonServices()
            .AddApplicationServices()
            .AddDatabaseServices(builder.Configuration)
            .AddStorageServices(builder.Configuration);

        // open telemetry services
        builder.Services.AddOpenTelemetry()
            .UseAzureMonitor(options =>
            {
                options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
                options.Credential = new DefaultAzureCredential();
            });

        return builder;
    }
}
