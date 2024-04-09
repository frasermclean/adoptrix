using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds;

public static class GetBreedEndpoint
{
    public const string EndpointName = "GetBreed";

    public static async Task<Results<Ok<BreedResponse>, NotFound>> ExecuteAsync(
        string breedIdOrName,
        IBreedsService breedsService,
        CancellationToken cancellationToken)
    {
        var result = Guid.TryParse(breedIdOrName, out var breedId)
            ? await breedsService.GetByIdAsync(breedId, cancellationToken)
            : await breedsService.GetByNameAsync(breedIdOrName, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value.ToResponse())
            : TypedResults.NotFound();
    }
}
