using System.Linq.Expressions;
using Adoptrix.Api.Mapping;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
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

        Expression<Func<Animal, bool>> wherePredicate = animalId.HasValue
            ? animal => animal.Id == animalId.Value
            : animal => animal.Slug == animalSlug;

        var response = await dbContext.Animals.Where(wherePredicate)
            .AsNoTracking()
            .Include(animal => animal.Breed)
            .ThenInclude(breed => breed.Species)
            .Select(animal => animal.ToResponse())
            .FirstOrDefaultAsync(cancellationToken);

        return response is not null
            ? TypedResults.Ok(response)
            : TypedResults.NotFound();
    }
}
