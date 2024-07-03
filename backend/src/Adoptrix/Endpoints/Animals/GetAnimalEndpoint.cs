using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Animals;

public class GetAnimalEndpoint(IAnimalsService animalsService) : EndpointWithoutRequest<Results<Ok<AnimalResponse>, NotFound>>
{
    public override void Configure()
    {
        Get("animals/{animalId}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var animalId = Route<Guid>("animalId");
        var result = await animalsService.GetAsync(animalId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }
}
