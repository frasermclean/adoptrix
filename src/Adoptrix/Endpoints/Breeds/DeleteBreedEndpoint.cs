using Adoptrix.Core.Abstractions;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Breeds;

[HttpDelete("breeds/{breedId:guid}")]
public class DeleteBreedEndpoint(IBreedsRepository breedsRepository)
    : Endpoint<DeleteBreedRequest, Results<NoContent, NotFound>>
{
    public override async Task<Results<NoContent, NotFound>> ExecuteAsync(DeleteBreedRequest request,
        CancellationToken cancellationToken)
    {
        var breed = await breedsRepository.GetByIdAsync(request.BreedId, cancellationToken);
        if (breed is null)
        {
            Logger.LogError("Could not delete breed with ID {BreedId} because it was not found", request.BreedId);
            return TypedResults.NotFound();
        }

        await breedsRepository.DeleteAsync(breed, cancellationToken);
        Logger.LogInformation("Breed with ID {BreedId} was deleted", request.BreedId);

        return TypedResults.NoContent();
    }
}
