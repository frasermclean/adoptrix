using Adoptrix.Api.Mapping;
using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Adoptrix.Persistence.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Animals;

[HttpGet("animals"), AllowAnonymous]
public class SearchAnimalsEndpoint(AdoptrixDbContext dbContext)
    : Endpoint<SearchAnimalsRequest, IEnumerable<AnimalMatch>>
{
    public override async Task<IEnumerable<AnimalMatch>> ExecuteAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken)
    {
        Sex? sex = Enum.TryParse<Sex>(request.Sex, true, out var value)
            ? value
            : null;

        var matches = await dbContext.Animals
            .AsNoTracking()
            .Where(animal => (request.Name == null || animal.Name.Contains(request.Name)) &&
                             (request.BreedId == null || animal.Breed.Id == request.BreedId) &&
                             (request.SpeciesName == null || animal.Breed.Species.Name == request.SpeciesName) &&
                             (sex == null || animal.Sex == sex))
            .Take(request.Limit ?? 10)
            .Select(animal => new AnimalMatch
            {
                Id = animal.Id,
                Name = animal.Name,
                SpeciesName = animal.Breed.Species.Name,
                BreedName = animal.Breed.Name,
                Slug = animal.Slug,
                Image = animal.Images.Select(image => image.ToResponse())
                    .FirstOrDefault()
            })
            .OrderBy(animal => animal.Name)
            .ToListAsync(cancellationToken);

        return matches;
    }
}
