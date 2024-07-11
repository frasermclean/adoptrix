using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Client;
using Adoptrix.Database.DependencyInjection;
using Adoptrix.Database.Services;
using Adoptrix.Storage.DependencyInjection;
using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Identity.Web;

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

        builder.Services
            .AddAuthentication(builder.Configuration)
            .AddFastEndpoints();

        // json serialization options
        builder.Services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });

        builder.Services.AddCommonServices()
            .AddDatabaseServices(builder.Configuration)
            .AddStorageServices(builder.Configuration);

        // open telemetry services
        builder.Services.AddOpenTelemetry()
            .UseAzureMonitor(options =>
            {
                options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
                options.Credential = new DefaultAzureCredential();
            });

        // health checks services
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<AdoptrixDbContext>()
            .AddAzureBlobStorage()
            .AddAzureQueueStorage();

        return builder;
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(jwtBearerOptions =>
            {
                configuration.Bind("Authentication", jwtBearerOptions);
                jwtBearerOptions.TokenValidationParameters.NameClaimType = ClaimConstants.Name;
            }, microsoftIdentityOptions => { configuration.Bind("Authentication", microsoftIdentityOptions); });

        services.AddAuthorizationBuilder()
            .AddDefaultPolicy("DefaultPolicy", builder => { builder.RequireScope("access"); });

        return services;
    }
}
