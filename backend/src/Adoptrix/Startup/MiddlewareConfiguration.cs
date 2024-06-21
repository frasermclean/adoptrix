using Adoptrix.Client.Pages;
using Adoptrix.Components;
using Adoptrix.Endpoints;

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

        app.MapGet("api/animals", AnimalsEndpoints.SearchAnimals);
        app.MapGet("api/animals/{animalId:guid}", AnimalsEndpoints.GetAnimal);

        return app;
    }
}
