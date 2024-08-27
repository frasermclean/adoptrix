using Adoptrix.Api.Security;
using Adoptrix.Logic.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public class DeleteAnimalEndpoint(IAnimalsService animalsService)
    : EndpointWithoutRequest<Results<NoContent, NotFound>>
{
    public override void Configure()
    {
        Delete("animals/{animalId:guid}");
        Permissions(PermissionNames.AnimalsWrite);
    }

    public override async Task<Results<NoContent, NotFound>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var animalId = Route<Guid>("animalId");

        var result = await animalsService.DeleteAsync(animalId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
