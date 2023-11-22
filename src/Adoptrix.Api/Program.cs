using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Application.Services;
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
            .AddApplicationServices()
            .AddInfrastructureServices();

        var app = builder.Build();

        // local development
        if (app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
            app.UseDeveloperExceptionPage();
        }

        app.UseFastEndpoints(config =>
        {
            config.Endpoints.Configurator = definition => definition.AllowAnonymous();
            config.Endpoints.RoutePrefix = "api";
        });
        app.Run();
    }
}