using Adoptrix.Api.Processors;
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

        app.UseFastEndpoints(config =>
        {
            config.Endpoints.RoutePrefix = ApiRoutePrefix;
            config.Endpoints.Configurator = definition =>
            {
                if (app.Configuration.GetValue<bool>("FastEndpoints:DisableAuthorization"))
                {
                    definition.AllowAnonymous();
                }

                var eventDispatcher = app.Services.GetRequiredService<EventDispatcherPostProcessor>();
                definition.PostProcessors(Order.After, eventDispatcher);
            };
        });

        app.MapHealthChecks($"{ApiRoutePrefix}/health").AllowAnonymous();

        return app;
    }
}