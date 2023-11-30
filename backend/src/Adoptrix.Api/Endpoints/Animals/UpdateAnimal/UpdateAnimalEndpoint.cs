using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Services;
using Adoptrix.Application.Commands.Animals;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals.UpdateAnimal;

public class UpdateAnimalEndpoint(IResponseMappingService mappingService)
    : Endpoint<UpdateAnimalCommand, Results<Ok<AnimalResponse>, NotFound>>
{
    public override void Configure()
    {
        Put("admin/animals/{id}");
    }

    public override async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(UpdateAnimalCommand command,
        CancellationToken cancellationToken)
    {
        var result = await command.ExecuteAsync(cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(mappingService.Map(result.Value))
            : TypedResults.NotFound();
    }
}