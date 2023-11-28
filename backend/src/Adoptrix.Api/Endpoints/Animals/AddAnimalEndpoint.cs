using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Application.Commands;
using Adoptrix.Application.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

[HttpPost("animals")]
public class AddAnimalEndpoint(ISqidConverter sqidConverter)
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

        var response = result.Value.ToResponse(sqidConverter);
        return TypedResults.Created($"api/animals/{response.Id}", response);
    }
}