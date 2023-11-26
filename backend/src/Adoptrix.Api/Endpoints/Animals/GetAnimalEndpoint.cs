using Adoptrix.Application.Commands;
using Adoptrix.Domain;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

[HttpGet("animals/{id}")]
public class GetAnimalEndpoint : Endpoint<GetAnimalCommand, Results<Ok<Animal>, NotFound>>
{
    public override async Task<Results<Ok<Animal>, NotFound>> ExecuteAsync(GetAnimalCommand command,
        CancellationToken cancellationToken)
    {
        var result = await command.ExecuteAsync(cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }
}