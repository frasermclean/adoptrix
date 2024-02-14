using Adoptrix.Api.Endpoints.Animals;
using FastEndpoints;

namespace Adoptrix.Api.Startup;

public static class MiddlewareConfiguration
{
    private const string ApiRoutePrefix = "api";

    /// <summary>
    /// Adds middleware to the application pipeline.
    /// </summary>
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        // local development middleware
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage()
                .UseCors();
        }

        // enable authentication and authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // map endpoints
        app.MapGroup("/minimal-api")
            .MapAnimalsEndpoints();

        app.UseFastEndpoints(config =>
        {
            config.Endpoints.RoutePrefix = ApiRoutePrefix;
            config.Endpoints.Configurator = definition =>
            {
                if (app.Configuration.GetValue<bool>("FastEndpoints:DisableAuthorization"))
                {
                    definition.AllowAnonymous();
                }
            };
        });

        app.MapHealthChecks($"{ApiRoutePrefix}/health").AllowAnonymous();

        return app;
    }

    private static RouteGroupBuilder MapAnimalsEndpoints(this RouteGroupBuilder builder)
    {
        var group = builder.MapGroup("animals");

        group.MapGet("", SearchAnimalsEndpoint.ExecuteAsync);
        group.MapGet("{animalId:guid}", GetAnimalEndpoint.ExecuteAsync);

        return builder;
    }
}