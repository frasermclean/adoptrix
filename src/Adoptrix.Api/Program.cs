using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Api.Endpoints;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Infrastructure;
using Adoptrix.Infrastructure.Services.Repositories;

namespace Adoptrix.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //builder.Services.AddFastEndpoints();
        // add application services
        builder.Services
            .AddDbContext<AdoptrixDbContext>()
            .ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            })
            .AddScoped<IAnimalsRepository, AnimalsRepository>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
            app.UseDeveloperExceptionPage();
        }

        app.MapGroup("/api")
            .MapAnimalEndpoints();

        //app.UseFastEndpoints(config => { config.Endpoints.RoutePrefix = "api"; });
        app.Run();
    }
}