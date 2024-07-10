using Adoptrix.Core.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Animals;

public class DeleteAnimalEndpoint(IAnimalsService animalsService)
    : Endpoint<DeleteAnimalRequest, Results<NoContent, NotFound>>
{
    public override void Configure()
    {
        Delete("animals/{animalId}");
    }

    public override async Task<Results<NoContent, NotFound>> ExecuteAsync(DeleteAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var result = await animalsService.DeleteAsync(request.AnimalId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
