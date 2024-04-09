using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Species;

public static class GetSpeciesEndpoint
{
    public const string EndpointName = "GetSpecies";

    public static async Task<Results<Ok<SpeciesResponse>, NotFound>> ExecuteAsync(
        string speciesIdOrName, ISpeciesService speciesService, CancellationToken cancellationToken)
    {
        var result = Guid.TryParse(speciesIdOrName, out var speciesId)
            ? await speciesService.GetByIdAsync(speciesId, cancellationToken)
            : await speciesService.GetByNameAsync(speciesIdOrName, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value.ToResponse())
            : TypedResults.NotFound();
    }
}
