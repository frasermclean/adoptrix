using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Api.Endpoints.Breeds;
using Adoptrix.Api.Endpoints.Species;
using Adoptrix.Api.Endpoints.Users;
using FastEndpoints;

namespace Adoptrix.Api.Startup;

public static class MiddlewareConfiguration
{
    private const string ApiRoutePrefix = "api";

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

        // map endpoints
        app.MapEndpoints();

        app.UseFastEndpoints(config =>
        {
            config.Endpoints.RoutePrefix = ApiRoutePrefix;
            config.Endpoints.Configurator = definition =>
            {
                if (app.Configuration.GetValue<bool>("FastEndpoints:DisableAuthorization"))
                {
                    definition.AllowAnonymous();
                }
            };
        });

        app.MapHealthChecks($"{ApiRoutePrefix}/health").AllowAnonymous();

        return app;
    }

    private static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var apiGroup = app.MapGroup("/api");

        var publicAnimalsGroup = apiGroup.MapGroup("/animals");
        publicAnimalsGroup.MapGet("/", SearchAnimalsEndpoint.ExecuteAsync);
        publicAnimalsGroup.MapGet("/{animalId:guid}", GetAnimalEndpoint.ExecuteAsync)
            .WithName(GetAnimalEndpoint.EndpointName);

        var adminGroup = apiGroup.MapGroup("/admin");
        adminGroup.MapPost("/animals", AddAnimalEndpoint.ExecuteAsync);
        adminGroup.MapPut("/animals/{animalId:guid}", UpdateAnimalEndpoint.ExecuteAsync);
        adminGroup.MapDelete("/animals/{animalId:guid}", DeleteAnimalEndpoint.ExecuteAsync);
        adminGroup.MapPost("/animals/{animalId:guid}/images", AddAnimalImagesEndpoint.ExecuteAsync)
            .DisableAntiforgery(); // TODO: Learn more about anti-forgery and remove this line
        adminGroup.RequireAuthorization();

        var breedsGroup = apiGroup.MapGroup("/breeds");
        breedsGroup.MapGet("{breedIdOrName}", GetBreedEndpoint.ExecuteAsync)
            .WithName(GetBreedEndpoint.EndpointName);
        breedsGroup.MapPost("/", AddBreedEndpoint.ExecuteAsync);
        breedsGroup.MapPut("{breedId:guid}", UpdateBreedEndpoint.ExecuteAsync);
        breedsGroup.MapDelete("{breedId:guid}", DeleteBreedEndpoint.ExecuteAsync);

        var speciesGroup = apiGroup.MapGroup("/species");
        speciesGroup.MapGet("", SearchSpeciesEndpoint.ExecuteAsync);

        var usersGroup = apiGroup.MapGroup("/users");
        usersGroup.MapGet("/me", GetCurrentUserEndpoint.Execute);
        usersGroup.RequireAuthorization();
    }
}
