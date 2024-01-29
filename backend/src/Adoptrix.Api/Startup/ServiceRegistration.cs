using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Api.Validators;
using Adoptrix.Application.Services;
using Adoptrix.Infrastructure;
using Adoptrix.Infrastructure.DependencyInjection;
using FastEndpoints;
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

        builder.Services
            .AddFastEndpoints()
            .AddAuthentication(builder.Configuration)
            .AddValidators()
            .AddApplicationServices()
            .AddInfrastructureServices(builder.Configuration);

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
            .AddMicrosoftIdentityWebApi(configuration);

        return services;
    }

    private static IServiceCollection AddDevelopmentServices(this IServiceCollection services)
    {
        // add cors policy for local development
        services.AddCors(options => options.AddDefaultPolicy(policyBuilder =>
            policyBuilder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
        ));
        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddSingleton<ImageContentTypeValidator>();
        services.AddSingleton<DateOfBirthValidator>();

        return services;
    }
}