using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Breeds;

[HttpGet("breeds"), AllowAnonymous]
public class SearchBreedsEndpoint(AdoptrixDbContext dbContext)
    : Endpoint<SearchBreedsRequest, IEnumerable<BreedMatch>>
{
    public override async Task<IEnumerable<BreedMatch>> ExecuteAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken)
    {
        var matches = await dbContext.Breeds
            .AsNoTracking()
            .Where(breed => (request.SpeciesName == null || breed.Species.Name == request.SpeciesName) &&
                            (request.WithAnimals == null || request.WithAnimals.Value && breed.Animals.Count > 0))
            .OrderBy(breed => breed.Name)
            .Select(breed => new BreedMatch
            {
                Id = breed.Id,
                Name = breed.Name,
                SpeciesName = breed.Species.Name,
                AnimalCount = breed.Animals.Count(animal => animal.Breed.Id == breed.Id)
            })
            .ToListAsync(cancellationToken);

        return matches;
    }
}
