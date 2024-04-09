using Adoptrix.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds;

public static class DeleteBreedEndpoint
{
    public static async Task<Results<NoContent, NotFound>> ExecuteAsync(
        Guid breedId,
        IBreedsService breedsService,
        CancellationToken cancellationToken)
    {
        var result = await breedsService.DeleteAsync(breedId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
