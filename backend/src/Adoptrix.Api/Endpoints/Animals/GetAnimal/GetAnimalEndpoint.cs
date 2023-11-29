using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Services;
using Adoptrix.Application.Commands;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals.GetAnimal;

[HttpGet("animals/{id}")]
public class GetAnimalEndpoint(IResponseMappingService mappingService)
    : Endpoint<GetAnimalCommand, Results<Ok<AnimalResponse>, NotFound>>
{
    public override async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(GetAnimalCommand command,
        CancellationToken cancellationToken)
    {
        var result = await command.ExecuteAsync(cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(mappingService.MapAnimal(result.Value))
            : TypedResults.NotFound();
    }
}