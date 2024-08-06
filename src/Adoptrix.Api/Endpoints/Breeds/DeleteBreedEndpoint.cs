using Adoptrix.Api.Security;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Breeds;

public class DeleteBreedEndpoint(AdoptrixDbContext dbContext)
    : EndpointWithoutRequest<Results<NoContent, NotFound>>
{
    public override void Configure()
    {
        Delete("breeds/{breedId:int}");
        Permissions(PermissionNames.BreedsWrite);
    }

    public override async Task<Results<NoContent, NotFound>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var breedId = Route<int>("breedId");

        var breed = await dbContext.Breeds
            .Include(breed => breed.Species)
            .Include(breed => breed.Animals)
            .FirstOrDefaultAsync(breed => breed.Id == breedId, cancellationToken);

        if (breed is null)
        {
            Logger.LogError("Could not delete breed with ID {BreedId} because it was not found", breedId);
            return TypedResults.NotFound();
        }

        dbContext.Breeds.Remove(breed);
        await dbContext.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Breed with ID {BreedId} was deleted", breedId);

        return TypedResults.NoContent();
    }
}
