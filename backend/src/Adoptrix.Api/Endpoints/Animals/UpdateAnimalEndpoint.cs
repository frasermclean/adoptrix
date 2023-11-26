using Adoptrix.Application.Commands;
using Adoptrix.Domain;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

[HttpPut("/animals/{id}")]
public class UpdateAnimalEndpoint : Endpoint<UpdateAnimalCommand, Results<Ok<Animal>, NotFound>>
{
    public override async Task<Results<Ok<Animal>, NotFound>> ExecuteAsync(UpdateAnimalCommand command,
        CancellationToken cancellationToken)
    {
        var result = await command.ExecuteAsync(cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }
}