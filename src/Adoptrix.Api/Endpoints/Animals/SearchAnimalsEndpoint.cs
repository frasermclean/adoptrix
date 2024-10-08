using Adoptrix.Persistence;
using Adoptrix.Persistence.Services;
using Gridify;
using Gridify.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Animals;

[HttpGet("animals"), AllowAnonymous]
public class SearchAnimalsEndpoint(
    AdoptrixDbContext dbContext,
    [FromKeyedServices(BlobContainerNames.AnimalImages)]
    IBlobContainerManager blobContainerManager)
    : Endpoint<GridifyQuery, Paging<SearchAnimalsItem>>
{
    public override async Task<Paging<SearchAnimalsItem>> ExecuteAsync(GridifyQuery query,
        CancellationToken cancellationToken)
    {
        return await dbContext.Animals
            .AsNoTracking()
            .Select(animal => new SearchAnimalsItem
            {
                Id = animal.Id,
                Slug = animal.Slug,
                Name = animal.Name,
                SpeciesName = animal.Breed.Species.Name,
                BreedName = animal.Breed.Name,
                Sex = animal.Sex,
                DateOfBirth = animal.DateOfBirth,
                PreviewImageUrl = animal.Images.Where(image => image.IsProcessed)
                    .Select(image => $"{blobContainerManager.ContainerUri}/{image.PreviewBlobName}")
                    .FirstOrDefault()
            })
            .GridifyAsync(query, cancellationToken);
    }
}
