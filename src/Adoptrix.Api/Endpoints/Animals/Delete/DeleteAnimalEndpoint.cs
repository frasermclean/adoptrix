using Adoptrix.Application.Commands.Animals;
using Adoptrix.Application.Services.Repositories;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals.Delete;

[HttpDelete("/animals/{id}")]
public class DeleteAnimalEndpoint(IAnimalsRepository repository)
    : Endpoint<DeleteAnimalCommand, Results<NoContent, NotFound>>
{
    public override async Task<Results<NoContent, NotFound>> ExecuteAsync(DeleteAnimalCommand command,
        CancellationToken cancellationToken)
    {
        var result = await command.ExecuteAsync(cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}