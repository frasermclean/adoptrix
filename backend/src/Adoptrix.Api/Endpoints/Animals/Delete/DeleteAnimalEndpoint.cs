using Adoptrix.Application.Commands;
using Adoptrix.Application.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals.Delete;

[HttpDelete("/animals/{id}")]
public class DeleteAnimalEndpoint(ISqidConverter sqidConverter)
    : Endpoint<DeleteAnimalRequest, Results<NoContent, NotFound>>
{
    public override async Task<Results<NoContent, NotFound>> ExecuteAsync(DeleteAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var command = new DeleteAnimalCommand { Id = sqidConverter.CovertToInt(request.Id) };
        var result = await command.ExecuteAsync(cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}