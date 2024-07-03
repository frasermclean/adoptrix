using Adoptrix.Domain.Contracts.Requests.Breeds;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Errors;
using Adoptrix.Domain.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Breeds;

public class UpdateBreedEndpoint(IBreedsService breedsService)
    : Endpoint<UpdateBreedRequest, Results<Ok<BreedResponse>, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Put("breeds/{breedId:guid}");
    }

    public override async Task<Results<Ok<BreedResponse>, NotFound, BadRequest>> ExecuteAsync(UpdateBreedRequest request, CancellationToken cancellationToken)
    {
        var result = await breedsService.UpdateAsync(request, cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        if (result.HasError<BreedNotFoundError>())
        {
            return TypedResults.NotFound();
        }

        return TypedResults.BadRequest();
    }
}
