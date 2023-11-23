using Adoptrix.Application.Commands;
using Adoptrix.Domain;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals.Add;

[HttpPost("animals")]
public class AddAnimalEndpoint : Endpoint<AddAnimalCommand, Results<Created<Animal>, UnprocessableEntity>>
{
    public override async Task<Results<Created<Animal>, UnprocessableEntity>> ExecuteAsync(AddAnimalCommand command,
        CancellationToken cancellationToken)
    {
        var result = await command.ExecuteAsync(cancellationToken);

        return result.IsSuccess
            ? TypedResults.Created($"api/animals/{result.Value.Id}", result.Value)
            : TypedResults.UnprocessableEntity();
    }
}