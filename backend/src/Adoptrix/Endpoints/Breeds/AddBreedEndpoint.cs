using Adoptrix.Domain.Contracts.Requests.Breeds;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Breeds;

public class AddBreedEndpoint(IBreedsService breedsService)
    : Endpoint<AddBreedRequest, Results<Created<BreedResponse>, BadRequest>>
{
    public override void Configure()
    {
        Post("breeds");
    }

    public override async Task<Results<Created<BreedResponse>, BadRequest>> ExecuteAsync(AddBreedRequest request,
        CancellationToken cancellationToken)
    {
        var result = await breedsService.AddAsync(request, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Created("", result.Value)
            : TypedResults.BadRequest();
    }
}
