using Adoptrix.Api.Endpoints;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Api.Endpoints.Breeds;
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



        var adminGroup = apiGroup.MapGroup("/admin");
        adminGroup.MapPost("/animals/{animalId:guid}/images", AddAnimalImagesEndpoint.ExecuteAsync)
            .DisableAntiforgery(); // TODO: Learn more about anti-forgery and remove this line
        adminGroup.RequireAuthorization();

        var breedsGroup = apiGroup.MapGroup("/breeds");
        breedsGroup.MapGet("/", SearchBreedsEndpoint.ExecuteAsync);
        breedsGroup.MapGet("/{breedIdOrName}", GetBreedEndpoint.ExecuteAsync)
            .WithName(GetBreedEndpoint.EndpointName);
        breedsGroup.MapPost("/", AddBreedEndpoint.ExecuteAsync);
        breedsGroup.MapPut("/{breedId:guid}", UpdateBreedEndpoint.ExecuteAsync);
        breedsGroup.MapDelete("/{breedId:guid}", DeleteBreedEndpoint.ExecuteAsync);

        var speciesGroup = apiGroup.MapGroup("/species");
        speciesGroup.MapGet("", GetAllSpeciesEndpoint.ExecuteAsync);
        speciesGroup.MapGet("/{speciesIdOrName}", GetSpeciesEndpoint.ExecuteAsync)
            .WithName(GetSpeciesEndpoint.EndpointName);

        var usersGroup = apiGroup.MapGroup("/users");
        usersGroup.MapGet("/me", GetCurrentUserEndpoint.Execute);
        usersGroup.RequireAuthorization();
    }
}
