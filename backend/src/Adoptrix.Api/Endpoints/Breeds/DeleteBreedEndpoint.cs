using Adoptrix.Application.Contracts.Requests.Breeds;
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
        var result = await sender.Send(new DeleteBreedRequest(breedId), cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
