using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Breeds;

public class GetBreedEndpoint(IBreedsService breedsService)
    : EndpointWithoutRequest<Results<Ok<BreedResponse>, NotFound>>
{
    public override void Configure()
    {
        Get("breeds/{breedId:guid}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<BreedResponse>, NotFound>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var breedId = Route<Guid>("breedId");
        var result = await breedsService.GetAsync(breedId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }
}
