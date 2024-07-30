using Adoptrix.Api.Endpoints;
using Adoptrix.ServiceDefaults;
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

        if (app.Environment.IsDevelopment())
        {
            app.UseCors();
        }

        app.UseFastEndpoints(config =>
        {
            config.Endpoints.RoutePrefix = "api";
            config.Security.PermissionsClaimType = ClaimTypes.Permission;
        });

        app.MapDefaultEndpoints();

        return app;
    }
}
