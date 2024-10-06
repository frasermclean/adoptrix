using Adoptrix.Core.Responses;
using Adoptrix.Logic.Mapping;
using Adoptrix.Persistence.Services;
using Gridify;
using Gridify.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Animals;

[HttpGet("animals"), AllowAnonymous]
public class SearchAnimalsEndpoint(AdoptrixDbContext dbContext)
    : Endpoint<GridifyQuery, Paging<AnimalMatch>>
{
    public override async Task<Paging<AnimalMatch>> ExecuteAsync(GridifyQuery query,
        CancellationToken cancellationToken)
    {
        return await dbContext.Animals
            .AsNoTracking()
            .Select(animal => new AnimalMatch
            {
                Id = animal.Id,
                Name = animal.Name,
                SpeciesName = animal.Breed.Species.Name,
                BreedName = animal.Breed.Name,
                Slug = animal.Slug,
                Image = animal.Images.Select(image => new AnimalImageResponse
                    {
                        Id = image.Id,
                        Description = image.Description,
                        IsProcessed = image.IsProcessed
                    })
                    .FirstOrDefault()
            })
            .GridifyAsync(query, cancellationToken);
    }
}
