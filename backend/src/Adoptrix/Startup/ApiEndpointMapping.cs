using Adoptrix.Endpoints;

namespace Adoptrix.Startup;

public static class ApiEndpointMapping
{
    public static WebApplication MapApiEndpoints(this WebApplication app)
    {
        var apiGroup = app.MapGroup("api");

        // animals
        apiGroup.MapGet("animals", AnimalsEndpoints.SearchAnimals);
        apiGroup.MapGet("animals/{animalId:guid}", AnimalsEndpoints.GetAnimal);

        // breeds
        apiGroup.MapGet("breeds", BreedsEndpoints.SearchBreeds);

        // species
        apiGroup.MapGet("species", SpeciesEndpoints.SearchSpecies);

        return app;
    }
}
