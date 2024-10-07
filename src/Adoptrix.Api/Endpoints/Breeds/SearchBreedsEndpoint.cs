using Adoptrix.Core.Responses;
using Adoptrix.Persistence.Services;
using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Breeds;

public class SearchBreedsEndpoint(AdoptrixDbContext dbContext) : Endpoint<GridifyQuery, Paging<BreedMatch>>
{
    public override void Configure()
    {
        Get("breeds");
        AllowAnonymous();
    }

    public override async Task<Paging<BreedMatch>> ExecuteAsync(GridifyQuery query,
        CancellationToken cancellationToken)
    {
        return await dbContext.Breeds
            .AsNoTracking()
            .Select(breed => new BreedMatch
            {
                Id = breed.Id,
                Name = breed.Name,
                SpeciesName = breed.Species.Name,
                AnimalCount = breed.Animals.Count
            })
            .GridifyAsync(query, cancellationToken);
    }
}
