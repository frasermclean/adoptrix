using Adoptrix.Core.Contracts.Requests.Animals;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Core.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Animals;

public class UpdateAnimalEndpoint(IAnimalsService animalsService)
    : Endpoint<UpdateAnimalRequest, Results<Ok<AnimalResponse>, NotFound>>
{
    public override void Configure()
    {
        Put("animals/{animalId:guid}");
    }

    public override async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(UpdateAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var result = await animalsService.UpdateAsync(request, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }
}
