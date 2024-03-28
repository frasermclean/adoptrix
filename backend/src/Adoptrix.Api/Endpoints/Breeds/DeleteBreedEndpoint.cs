using Adoptrix.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds;

public static class DeleteBreedEndpoint
{
    public static async Task<Results<NoContent, NotFound>> ExecuteAsync(
        Guid breedId,
        IBreedsRepository breedsRepository,
        CancellationToken cancellationToken)
    {
        var result = await breedsRepository.DeleteAsync(breedId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
