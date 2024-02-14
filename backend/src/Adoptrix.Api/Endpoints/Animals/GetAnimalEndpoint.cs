using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Services.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public static class GetAnimalEndpoint
{
    public static async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(Guid animalId, IAnimalsRepository repository,
        CancellationToken cancellationToken)
    {
        var result = await repository.GetAsync(animalId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value.ToResponse())
            : TypedResults.NotFound();
    }
}