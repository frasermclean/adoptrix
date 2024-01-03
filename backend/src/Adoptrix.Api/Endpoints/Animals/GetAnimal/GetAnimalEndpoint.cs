using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Commands.Animals;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals.GetAnimal;

public class GetAnimalEndpoint : Endpoint<GetAnimalCommand, Results<Ok<AnimalResponse>, NotFound>>
{
    public override void Configure()
    {
        Get("animals/{id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(GetAnimalCommand command,
        CancellationToken cancellationToken)
    {
        var result = await command.ExecuteAsync(cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value.ToResponse())
            : TypedResults.NotFound();
    }
}