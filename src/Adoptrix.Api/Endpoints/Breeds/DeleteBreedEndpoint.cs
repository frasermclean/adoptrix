using Adoptrix.Api.Security;
using Adoptrix.Persistence.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Breeds;

public class DeleteBreedEndpoint(AdoptrixDbContext dbContext) : EndpointWithoutRequest<Results<NoContent, NotFound>>
{
    public override void Configure()
    {
        Delete("breeds/{breedId:int}");
        Permissions(PermissionNames.BreedsWrite);
    }

    public override async Task<Results<NoContent, NotFound>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var breedId = Route<int>("breedId");

        var deletedRowCount = await dbContext.Breeds.Where(breed => breed.Id == breedId)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRowCount == 0)
        {
            Logger.LogError("Failed to delete breed with ID {BreedId}", breedId);
            return TypedResults.NotFound();
        }

        Logger.LogInformation("Deleted breed with ID {BreedId}", breedId);
        return TypedResults.NoContent();
    }
}
