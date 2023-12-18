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

        // enable authentication and authorization
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseFastEndpoints(config =>
        {
            config.Endpoints.RoutePrefix = "api";
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

        return app;
    }
}