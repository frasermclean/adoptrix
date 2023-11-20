using Adoptrix.Api.Contracts;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints;

public static class AnimalEndpoints
{
    public static RouteGroupBuilder MapAnimalEndpoints(this RouteGroupBuilder builder)
    {
        builder.MapGet("/animals", SearchAnimalsAsync);
        builder.MapGet("/animals/{animalId}", GetAnimalAsync);
        builder.MapPost("/animals", AddAnimalAsync);
        builder.MapDelete("/animals/{animalId}", DeleteAnimalAsync);

        return builder;
    }

    private static async Task<IEnumerable<Animal>> SearchAnimalsAsync(IAnimalsRepository repository,
        string? name = null, Species? species = null, CancellationToken cancellationToken = default)
    {
        return await repository.SearchAsync(name, species, cancellationToken);
    }

    private static async Task<Results<Ok<Animal>, NotFound>> GetAnimalAsync(Guid animalId,
        IAnimalsRepository repository, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(animalId, cancellationToken);
        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }

    private static async Task<Results<Created<Animal>, BadRequest>> AddAnimalAsync(AddAnimalRequest request,
        IAnimalsRepository repository, CancellationToken cancellationToken = default)
    {
        var animal = await repository.AddAsync(new Animal
        {
            Name = request.Name,
            Species = request.Species,
            DateOfBirth = request.DateOfBirth
        }, cancellationToken);

        var uri = new Uri($"/api/animals/{animal.Id}", UriKind.Relative);
        return TypedResults.Created(uri, animal);
    }

    private static async Task<Results<NoContent, NotFound>> DeleteAnimalAsync(Guid animalId,
        IAnimalsRepository repository, CancellationToken cancellationToken = default)
    {
        var result = await repository.DeleteAsync(animalId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}