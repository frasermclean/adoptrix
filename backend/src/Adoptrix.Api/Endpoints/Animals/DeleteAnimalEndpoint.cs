using Adoptrix.Application.Features.Animals.Commands;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public sealed class DeleteAnimalEndpoint
{
    public static async Task<Results<NoContent, NotFound>> ExecuteAsync(Guid animalId, ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteAnimalCommand(animalId), cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
