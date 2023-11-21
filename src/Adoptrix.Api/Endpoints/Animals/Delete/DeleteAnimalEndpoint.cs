using Adoptrix.Application.Services.Repositories;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals.Delete;

[HttpDelete("/animals/{id}")]
public class DeleteAnimalEndpoint(IAnimalsRepository repository)
    : Endpoint<DeleteAnimalsRequest, Results<NoContent, NotFound>>
{
    public override async Task<Results<NoContent, NotFound>> ExecuteAsync(DeleteAnimalsRequest request,
        CancellationToken cancellationToken)
    {
        var result = await repository.DeleteAsync(request.Id, cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}