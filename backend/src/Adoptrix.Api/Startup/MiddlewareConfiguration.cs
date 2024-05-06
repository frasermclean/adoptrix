using Adoptrix.Api.Endpoints;

namespace Adoptrix.Api.Startup;

public static class MiddlewareConfiguration
{
    /// <summary>
    /// Adds middleware to the application pipeline.
    /// </summary>
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseStatusCodePages();

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
        app.MapControllers();
        app.MapEndpoints();
        app.MapHealthChecks("/health").AllowAnonymous();

        return app;
    }

    private static void MapEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var apiGroup = routeBuilder.MapGroup("api");

        var assistantsGroup = apiGroup.MapGroup("assistants");
        assistantsGroup.MapGet("animals/description", AssistantEndpoints.GenerateAnimalDescription);
    }
}
