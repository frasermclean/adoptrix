using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;
using Adoptrix.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Species;

[HttpGet("species"), AllowAnonymous]
public class SearchSpeciesEndpoint(AdoptrixDbContext dbContext)
    : Endpoint<SearchSpeciesRequest, IEnumerable<SpeciesMatch>>
{
    public override async Task<IEnumerable<SpeciesMatch>> ExecuteAsync(SearchSpeciesRequest request,
        CancellationToken cancellationToken)
    {
        var matches = await dbContext.Species
            .Select(species => new SpeciesMatch
            {
                Name = species.Name,
                BreedCount = species.Breeds.Count,
                AnimalCount = species.Breeds.Count(breed => breed.Animals.Count > 0)
            })
            .Where(match => request.WithAnimals == null || !request.WithAnimals.Value || match.AnimalCount > 0)
            .OrderByDescending(match => match.AnimalCount)
            .ToListAsync(cancellationToken);

        return matches;
    }
}
