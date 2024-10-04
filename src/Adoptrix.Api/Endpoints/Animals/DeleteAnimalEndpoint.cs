using Adoptrix.Api.Security;
using Adoptrix.Core.Requests;
using Adoptrix.Logic.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public class DeleteAnimalEndpoint(IAnimalsService animalsService)
    : Endpoint<DeleteAnimalRequest, Results<NoContent, NotFound>>
{
    public override void Configure()
    {
        Delete("animals/{animalId:guid}");
        Permissions(PermissionNames.AnimalsWrite);
    }

    public override async Task<Results<NoContent, NotFound>> ExecuteAsync(DeleteAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var result = await animalsService.DeleteAsync(request, cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
