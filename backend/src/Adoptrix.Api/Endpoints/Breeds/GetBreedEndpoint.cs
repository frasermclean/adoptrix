using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Services.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds;

public static class GetBreedEndpoint
{
    public static async Task<Results<Ok<BreedResponse>, NotFound<NotFoundResponse>>> ExecuteAsync(
        string breedIdOrName,
        IBreedsRepository breedsRepository,
        CancellationToken cancellationToken)
    {
        var result = Guid.TryParse(breedIdOrName, out var breedId)
            ? await breedsRepository.GetByIdAsync(breedId, cancellationToken)
            : await breedsRepository.GetByNameAsync(breedIdOrName, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value.ToResponse())
            : TypedResults.NotFound(NotFoundResponse.Create(result.GetFirstErrorMessage()));
    }
}
