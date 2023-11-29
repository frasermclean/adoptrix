using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Services;
using Adoptrix.Application.Commands;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals.AddAnimal;

[HttpPost("animals")]
public class AddAnimalEndpoint(IResponseMappingService mappingService)
    : Endpoint<AddAnimalCommand, Results<Created<AnimalResponse>, UnprocessableEntity>>
{
    public override async Task<Results<Created<AnimalResponse>, UnprocessableEntity>> ExecuteAsync(
        AddAnimalCommand command, CancellationToken cancellationToken)
    {
        var result = await command.ExecuteAsync(cancellationToken);

        if (result.IsFailed)
        {
            return TypedResults.UnprocessableEntity();
        }

        var response = mappingService.MapAnimal(result.Value);
        return TypedResults.Created($"api/animals/{response.Id}", response);
    }
}