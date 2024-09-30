using Adoptrix.Core.Responses;
using Adoptrix.Logic.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public class GetAnimalEndpoint(IAnimalsService animalsService)
    : EndpointWithoutRequest<Results<Ok<AnimalResponse>, NotFound>>
{
    public override void Configure()
    {
        Get("animals/{animalId:guid}", "animals/{animalSlug}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var animalId = Route<Guid?>("animalId", false);
        var animalSlug = Route<string>("animalSlug", false);

        var result = animalId.HasValue
            ? await animalsService.GetAsync(animalId.Value, cancellationToken)
            : await animalsService.GetAsync(animalSlug!, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }
}
