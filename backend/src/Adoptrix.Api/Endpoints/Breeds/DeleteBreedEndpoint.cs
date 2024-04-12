using Adoptrix.Application.Features.Breeds.Commands;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds;

public static class DeleteBreedEndpoint
{
    public static async Task<Results<NoContent, NotFound>> ExecuteAsync(
        Guid breedId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteBreedCommand(breedId), cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
