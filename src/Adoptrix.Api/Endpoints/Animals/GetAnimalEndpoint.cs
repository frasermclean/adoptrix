using Adoptrix.Api.Mapping;
using Adoptrix.Contracts.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Animals;

public class GetAnimalEndpoint(AdoptrixDbContext dbContext)
    : EndpointWithoutRequest<Results<Ok<AnimalResponse>, NotFound>>
{
    public override void Configure()
    {
        Get("animals/{animalId:int}", "animals/{animalSlug}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var animalId = Route<int?>("animalId", false);
        var animalSlug = Route<string>("animalSlug", false);

        var response = animalId.HasValue
            ? await GetByIdAsync(animalId.Value, cancellationToken)
            : await GetBySlugAsync(animalSlug, cancellationToken);

        return response is not null
            ? TypedResults.Ok(response)
            : TypedResults.NotFound();
    }

    private async Task<AnimalResponse?> GetByIdAsync(int animalId, CancellationToken cancellationToken) =>
        await dbContext.Animals.Where(animal => animal.Id == animalId)
            .AsNoTracking()
            .Include(animal => animal.Breed)
            .ThenInclude(breed => breed.Species)
            .Select(animal => animal.ToResponse())
            .FirstOrDefaultAsync(cancellationToken);

    private async Task<AnimalResponse?> GetBySlugAsync(string? animalSlug, CancellationToken cancellationToken) =>
        await dbContext.Animals.Where(animal => animal.Slug == animalSlug)
            .AsNoTracking()
            .Include(animal => animal.Breed)
            .ThenInclude(breed => breed.Species)
            .Select(animal => animal.ToResponse())
            .FirstOrDefaultAsync(cancellationToken);
}
