using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public static class GetAnimalEndpoint
{
    public static async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(Guid animalId, IAnimalsService animalsService,
        CancellationToken cancellationToken)
    {
        var result = await animalsService.GetAnimalByIdAsync(animalId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value.ToResponse())
            : TypedResults.NotFound();
    }
}
