using Adoptrix.ServiceDefaults;
using Adoptrix.Web.Components;

namespace Adoptrix.Web.Startup;

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
        app.UseRouting();
        app.UseAntiforgery();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.MapControllers();
        app.MapDefaultEndpoints();

        return app;
    }
}
