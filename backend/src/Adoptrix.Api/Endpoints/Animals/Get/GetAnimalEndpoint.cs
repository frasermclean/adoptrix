using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Application.Commands;
using Adoptrix.Application.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals.Get;

[HttpGet("animals/{id}")]
public class GetAnimalEndpoint(ISqidConverter sqidConverter)
    : Endpoint<GetAnimalRequest, Results<Ok<AnimalResponse>, NotFound>>
{
    public override async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(GetAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var command = new GetAnimalCommand { Id = sqidConverter.CovertToInt(request.Id) };
        var result = await command.ExecuteAsync(cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value.ToResponse(sqidConverter))
            : TypedResults.NotFound();
    }
}