using Adoptrix.ServiceDefaults;
using FastEndpoints;
using Microsoft.Identity.Web;

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
            config.Security.RoleClaimType = ClaimConstants.Roles;
            config.Security.PermissionsClaimType = "permissions";
        });

        app.MapDefaultEndpoints();

        return app;
    }
}
