using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Services;
using Adoptrix.Application.Commands.Animals;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals.AddAnimal;

public class AddAnimalEndpoint(IResponseMappingService mappingService)
    : Endpoint<AddAnimalCommand, Results<Created<AnimalResponse>, BadRequest>>
{
    public override void Configure()
    {
        Post("admin/animals");
    }

    public override async Task<Results<Created<AnimalResponse>, BadRequest>> ExecuteAsync(
        AddAnimalCommand command, CancellationToken cancellationToken)
    {
        var result = await command.ExecuteAsync(cancellationToken);

        if (result.IsFailed)
        {
            return TypedResults.BadRequest();
        }

        var response = mappingService.Map(result.Value);
        return TypedResults.Created($"api/animals/{response.Id}", response);
    }
}