using Adoptrix.Core.Responses;
using Adoptrix.Persistence.Services;
using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Species;

public class SearchSpeciesEndpoint(AdoptrixDbContext dbContext)
    : Endpoint<GridifyQuery, Paging<SpeciesResponse>>
{
    public override void Configure()
    {
        Get("species");
        AllowAnonymous();
    }

    public override async Task<Paging<SpeciesResponse>> ExecuteAsync(GridifyQuery query,
        CancellationToken cancellationToken)
    {
        return await dbContext.Species
            .AsNoTracking()
            .Select(species => new SpeciesResponse
            {
                Id = species.Id,
                Name = species.Name,
                BreedCount = species.Breeds.Count,
                AnimalCount = species.Breeds.Count(breed => breed.Animals.Count > 0)
            })
            .GridifyAsync(query, cancellationToken);
    }
}
