using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Api.Validators;
using Adoptrix.Application.DependencyInjection;
using Adoptrix.Database.DependencyInjection;
using Adoptrix.Database.Services;
using Adoptrix.Storage.DependencyInjection;
using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Identity.Web;

namespace Adoptrix.Api.Startup;

public static class ServiceRegistration
{
    /// <summary>
    /// Registers services within the dependency injection container.
    /// </summary>
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        // json serialization options
        builder.Services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });

        // open telemetry services
        builder.Services.AddOpenTelemetry()
            .UseAzureMonitor(options =>
            {
                options.ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
                options.Credential = new DefaultAzureCredential();
            });

        builder.Services
            .AddAuthentication(builder.Configuration)
            .AddValidatorsFromAssemblyContaining<SetAnimalDataValidator>()
            .AddProblemDetails()
            .AddApplicationServices()
            .AddDatabaseServices()
            .AddStorageServices(builder.Configuration);

        // local development services
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddDevelopmentServices();
        }

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

    private static void AddDevelopmentServices(this IServiceCollection services)
    {
        // add cors policy for local development
        services.AddCors(options => options.AddDefaultPolicy(policyBuilder =>
            policyBuilder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
        ));
    }
}
