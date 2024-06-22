using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Animals;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints;

public static class AnimalsEndpoints
{
    public static async Task<IEnumerable<AnimalMatch>> SearchAnimals([AsParameters] SearchAnimalsQuery query,
        ISender sender, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }

    public static async Task<Results<Ok<AnimalResponse>, NotFound>> GetAnimal(Guid animalId, ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAnimalQuery(animalId), cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }
}
