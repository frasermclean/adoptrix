using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Application.Commands;
using Adoptrix.Application.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals.Update;

[HttpPut("/animals/{id}")]
public class UpdateAnimalEndpoint(ISqidConverter sqidConverter)
    : Endpoint<UpdateAnimalRequest, Results<Ok<AnimalResponse>, NotFound>>
{
    public override async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(UpdateAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var command = MapToCommand(request);
        var result = await command.ExecuteAsync(cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value.ToResponse(sqidConverter))
            : TypedResults.NotFound();
    }

    private UpdateAnimalCommand MapToCommand(UpdateAnimalRequest request) => new()
    {
        Id = sqidConverter.CovertToInt(request.Id),
        Name = request.Name,
        Description = request.Description,
        Species = request.Species,
        DateOfBirth = request.DateOfBirth
    };
}