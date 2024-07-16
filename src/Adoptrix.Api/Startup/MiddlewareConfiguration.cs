using FastEndpoints;

namespace Adoptrix.Api.Startup;

public static class MiddlewareConfiguration
{
    /// <summary>
    /// Configure the HTTP request pipeline.
    /// </summary>
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseHsts();

        app.UseFastEndpoints(config =>
        {
            config.Endpoints.RoutePrefix = "api";
        });

        app.MapHealthChecks("/health")
            .AllowAnonymous();

        return app;
    }
}
