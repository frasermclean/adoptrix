using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Contracts.Requests.Animals;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public static class GetAnimalEndpoint
{
    public const string EndpointName = "GetAnimal";

    public static async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(Guid animalId,
        ISender sender, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAnimalRequest(animalId), cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value.ToResponse())
            : TypedResults.NotFound();
    }
}
