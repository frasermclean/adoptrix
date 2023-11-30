using Adoptrix.Api.Extensions;
using Adoptrix.Application.Commands.Breeds;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds.DeleteBreed;

[HttpDelete("/breeds/{id}")]
public class DeleteBreedEndpoint : Endpoint<DeleteBreedCommand, Results<NoContent, NotFound<string>>>
{
    public override async Task<Results<NoContent, NotFound<string>>> ExecuteAsync(DeleteBreedCommand command,
        CancellationToken cancellationToken)
    {
        var result = await command.ExecuteAsync(cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : TypedResults.NotFound(result.GetFirstErrorMessage());
    }
}