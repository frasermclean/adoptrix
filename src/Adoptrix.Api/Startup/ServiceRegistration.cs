﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Api.Processors;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Services;
using Adoptrix.Infrastructure.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.Json;

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
            .AddDomainServices()
            .AddApplicationServices()
            .AddInfrastructureServices()
            .AddDevelopmentServices(builder.Environment)
            .AddSingleton<EventDispatcherPostProcessor>();

        return builder;
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
        {
            policyBuilder.AllowAnyOrigin();
            policyBuilder.AllowAnyHeader();
            policyBuilder.AllowAnyMethod();
        }));

        return services;
    }
}