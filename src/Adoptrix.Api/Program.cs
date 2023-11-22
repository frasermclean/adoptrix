using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Api.Processors;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Services;
using Adoptrix.Infrastructure.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.Json;

namespace Adoptrix.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // add application services
        builder.Services
            .Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            })
            .AddFastEndpoints()
            .AddDomainServices()
            .AddApplicationServices()
            .AddInfrastructureServices()
            .AddSingleton<EventDispatcherPostProcessor>();

        var app = builder.Build();

        // add middleware
        app.UseFastEndpoints(config =>
        {
            config.Endpoints.RoutePrefix = "api";
            config.Endpoints.Configurator = definition =>
            {
                definition.AllowAnonymous(); // TODO: Implement authentication and authorization
                definition.PostProcessors(Order.After, app.Services.GetRequiredService<EventDispatcherPostProcessor>());
            };
        });

        app.Run();
    }
}