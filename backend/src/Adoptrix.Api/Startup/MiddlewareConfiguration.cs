using Adoptrix.Api.Endpoints;
using Adoptrix.Api.Endpoints.Species;
using Adoptrix.Api.Endpoints.Users;

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

    private static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var apiGroup = app.MapGroup("/api");

        apiGroup.MapGet("/about", AboutEndpoint.Execute)
            .AllowAnonymous()
            .WithName("About");

        var speciesGroup = apiGroup.MapGroup("/species");
        speciesGroup.MapGet("", GetAllSpeciesEndpoint.ExecuteAsync);
        speciesGroup.MapGet("/{speciesIdOrName}", GetSpeciesEndpoint.ExecuteAsync)
            .WithName(GetSpeciesEndpoint.EndpointName);

        var usersGroup = apiGroup.MapGroup("/users");
        usersGroup.MapGet("/me", GetCurrentUserEndpoint.Execute);
        usersGroup.RequireAuthorization();
    }
}
