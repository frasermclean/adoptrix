using Adoptrix.Domain.Commands.Animals;
using Adoptrix.Domain.Contracts.Requests.Animals;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Animals;

public class UpdateAnimalEndpoint(IAnimalsService animalsService)
    : Endpoint<UpdateAnimalRequest, Results<Ok<AnimalResponse>, NotFound>>
{
    public override void Configure()
    {
        Put("animals/{animalId}");
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
