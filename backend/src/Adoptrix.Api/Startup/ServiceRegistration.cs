﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Api.Processors;
using Adoptrix.Api.Services;
using Adoptrix.Api.Validators;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Services;
using Adoptrix.Infrastructure;
using Adoptrix.Infrastructure.Services;
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
            .AddDomainServices()
            .AddApplicationServices(builder.Configuration)
            .AddInfrastructureServices(builder.Configuration, builder.Environment)
            .AddDevelopmentServices(builder.Environment)
            .AddScoped<IResponseMappingService, ResponseMappingService>()
            .AddSingleton<EventDispatcherPostProcessor>();

        // health checks services
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<AdoptrixDbContext>();

        return builder;
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration);

        return services;
    }

    private static IServiceCollection AddDevelopmentServices(this IServiceCollection services,
        IHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
        {
            return services;
        }

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
        services.AddSingleton<SqidValidator>();
        services.AddSingleton<ImageContentTypeValidator>();
        services.AddSingleton<DateOfBirthValidator>();

        return services;
    }
}