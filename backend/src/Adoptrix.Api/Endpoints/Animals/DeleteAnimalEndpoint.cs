using Adoptrix.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public static class DeleteAnimalEndpoint
{
    public static async Task<Results<NoContent, NotFound>> ExecuteAsync(Guid animalId, IAnimalsService animalsService,
        CancellationToken cancellationToken)
    {
        var result = await animalsService.DeleteAnimalAsync(animalId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
