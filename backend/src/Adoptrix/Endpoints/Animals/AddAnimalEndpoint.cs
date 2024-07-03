using Adoptrix.Domain.Commands.Animals;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Animals;

public class AddAnimalEndpoint(IAnimalsService animalsService) : Endpoint<AddAnimalCommand, Results<Created<AnimalResponse>, BadRequest>>
{
    public override void Configure()
    {
        Post("animals");
    }

    public override async Task<Results<Created<AnimalResponse>, BadRequest>> ExecuteAsync(AddAnimalCommand command, CancellationToken cancellationToken)
    {
        var result = await animalsService.AddAsync(command, cancellationToken);

        return TypedResults.Created("", result.Value); // TODO: Add location header
    }
}
