using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Services;
using Adoptrix.Application.Commands.Breeds;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds.AddBreed;

[HttpPost("breeds")]
public class AddBreedEndpoint(IResponseMappingService mappingService)
    : Endpoint<AddBreedCommand, Results<Created<BreedResponse>, BadRequest<string>>>
{
    public override async Task<Results<Created<BreedResponse>, BadRequest<string>>> ExecuteAsync(
        AddBreedCommand command, CancellationToken cancellationToken)
    {
        var result = await command.ExecuteAsync(cancellationToken);

        return result.IsSuccess
            ? TypedResults.Created($"breeds/{result.Value.Id}", mappingService.Map(result.Value))
            : TypedResults.BadRequest(result.Errors.First().Message);
    }
}