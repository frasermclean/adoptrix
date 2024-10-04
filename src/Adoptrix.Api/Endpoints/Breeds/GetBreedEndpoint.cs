using Adoptrix.Core.Responses;
using Adoptrix.Logic.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds;

public class GetBreedEndpoint(IBreedsService breedsService)
    : EndpointWithoutRequest<Results<Ok<BreedResponse>, NotFound>>
{
    public override void Configure()
    {
        Get("breeds/{breedId:int}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<BreedResponse>, NotFound>> ExecuteAsync(
        CancellationToken cancellationToken)
    {
        var breedId = Route<int>("breedId");

        var result = await breedsService.GetAsync(breedId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }
}
