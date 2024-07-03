using Adoptrix.Domain.Commands.Animals;
using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Animals;

public class UpdateAnimalEndpoint(IAnimalsService animalsService)
    : Endpoint<UpdateAnimalCommand, Results<Ok<AnimalResponse>, NotFound>>
{
    public override void Configure()
    {
        Put("animals/{animalId}");
    }

    public override async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(UpdateAnimalCommand command,
        CancellationToken cancellationToken)
    {
        var result = await animalsService.UpdateAsync(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }
}
