using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Services;
using Adoptrix.Application.Commands.Breeds;
using Adoptrix.Domain.Errors;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds.UpdateBreed;

[HttpPut("/breeds/{id}")]
public class UpdateBreedEndpoint(IResponseMappingService mappingService)
    : Endpoint<UpdateBreedCommand, Results<Ok<BreedResponse>, BadRequest<string>, NotFound<string>>>
{
    public override async Task<Results<Ok<BreedResponse>, BadRequest<string>, NotFound<string>>> ExecuteAsync(
        UpdateBreedCommand command, CancellationToken cancellationToken)
    {
        var result = await command.ExecuteAsync(cancellationToken);

        if (result.HasError<BreedNotFoundError>())
        {
            return TypedResults.NotFound(result.GetFirstErrorMessage());
        }

        return result.IsSuccess
            ? TypedResults.Ok(mappingService.Map(result.Value))
            : TypedResults.BadRequest(result.GetFirstErrorMessage());
    }
}