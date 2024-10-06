using System.Linq.Expressions;
using Adoptrix.Core;
using Adoptrix.Core.Responses;
using Adoptrix.Persistence.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Animals;

public class GetAnimalEndpoint(AdoptrixDbContext dbContext)
    : EndpointWithoutRequest<Results<Ok<AnimalResponse>, NotFound>>
{
    public override void Configure()
    {
        Get("animals/{animalId:guid}", "animals/{animalSlug}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var animalId = Route<Guid?>("animalId", false);
        var animalSlug = Route<string>("animalSlug", false);

        Expression<Func<Animal, bool>> predicate = animalId.HasValue
            ? animal => animal.Id == animalId
            : animal => animal.Slug == animalSlug;

        var response = await dbContext.Animals.Where(predicate)
            .Select(animal => new AnimalResponse
            {
                Id = animal.Id,
                Name = animal.Name,
                Description = animal.Description,
                SpeciesName = animal.Breed.Species.Name,
                BreedName = animal.Breed.Name,
                Sex = animal.Sex,
                DateOfBirth = animal.DateOfBirth,
                Slug = animal.Slug,
                Age = "",
                LastModifiedUtc = animal.LastModifiedUtc,
                Images = animal.Images.Select(image => new AnimalImageResponse
                {
                    Id = image.Id,
                    Description = image.Description,
                    IsProcessed = image.IsProcessed
                })
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return response is not null
            ? TypedResults.Ok(response)
            : TypedResults.NotFound();
    }
}
