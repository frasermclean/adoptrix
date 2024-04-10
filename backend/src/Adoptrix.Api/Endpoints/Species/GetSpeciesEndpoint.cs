using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Contracts.Requests.Species;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Species;

public static class GetSpeciesEndpoint
{
    public const string EndpointName = "GetSpecies";

    public static async Task<Results<Ok<SpeciesResponse>, NotFound>> ExecuteAsync(
        string speciesIdOrName, ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetSpeciesRequest(speciesIdOrName), cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value.ToResponse())
            : TypedResults.NotFound();
    }
}
