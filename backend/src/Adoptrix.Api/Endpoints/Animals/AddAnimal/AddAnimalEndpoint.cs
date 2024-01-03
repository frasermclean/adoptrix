using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Commands.Animals;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals.AddAnimal;

public class AddAnimalEndpoint : Endpoint<AddAnimalCommand, Results<Created<AnimalResponse>, BadRequest>>
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

        var response = result.Value.ToResponse();
        return TypedResults.Created($"api/animals/{response.Id}", response);
    }
}