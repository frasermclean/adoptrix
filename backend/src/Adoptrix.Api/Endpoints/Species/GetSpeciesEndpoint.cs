using Adoptrix.Persistence.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Species;

public class GetSpeciesEndpoint(AdoptrixDbContext dbContext)
    : EndpointWithoutRequest<Results<Ok<SpeciesResponse>, NotFound>>
{
    public override void Configure()
    {
        Get("species/{speciesName}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<SpeciesResponse>, NotFound>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var speciesName = Route<string>("speciesName");

        var response = await dbContext.Species.Where(species => species.Name == speciesName)
            .Select(species => new SpeciesResponse
            {
                Id = species.Id,
                Name = species.Name,
                BreedCount = species.Breeds.Count,
                AnimalCount = species.Breeds.Count(breed => breed.Animals.Count > 0)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return response is not null
            ? TypedResults.Ok(response)
            : TypedResults.NotFound();
    }
}
