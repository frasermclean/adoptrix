using Adoptrix.Core.Responses;
using Adoptrix.Logic.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Species;

public class GetSpeciesEndpoint(ISpeciesService speciesService)
    : EndpointWithoutRequest<Results<Ok<SpeciesResponse>, NotFound>>
{
    public override void Configure()
    {
        Get("species/{speciesName}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<SpeciesResponse>, NotFound>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var speciesName = Route<string>("speciesName");

        var result = await speciesService.GetAsync(speciesName!, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }
}
