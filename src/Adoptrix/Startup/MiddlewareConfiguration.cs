using Adoptrix.Client.Pages;
using Adoptrix.Components;
using FastEndpoints;

namespace Adoptrix.Startup;

public static class MiddlewareConfiguration
{
    /// <summary>
    /// Configure the HTTP request pipeline.
    /// </summary>
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Home).Assembly);

        app.UseFastEndpoints(config =>
        {
            config.Endpoints.RoutePrefix = "api";
        });

        app.MapHealthChecks("/health")
            .AllowAnonymous();

        return app;
    }
}
