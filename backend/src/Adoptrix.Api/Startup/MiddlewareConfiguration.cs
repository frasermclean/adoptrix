using Adoptrix.Api.Processors;
using FastEndpoints;

namespace Adoptrix.Api.Startup;

public static class MiddlewareConfiguration
{
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

        app.UseFastEndpoints(config =>
        {
            config.Endpoints.RoutePrefix = "api";
            config.Endpoints.Configurator = definition =>
            {
                definition.AllowAnonymous(); // TODO: Implement authentication and authorization
                definition.PostProcessors(Order.After, app.Services.GetRequiredService<EventDispatcherPostProcessor>());
            };
        });

        return app;
    }
}