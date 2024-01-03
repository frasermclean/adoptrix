using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Commands.Breeds;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds.GetBreed;

public class GetBreedEndpoint : Endpoint<GetBreedCommand, Results<Ok<BreedResponse>, NotFound<string>>>
{
    public override void Configure()
    {
        Get("breeds/{id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<BreedResponse>, NotFound<string>>> ExecuteAsync(GetBreedCommand command,
        CancellationToken cancellationToken)
    {
        var result = await command.ExecuteAsync(cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value.ToResponse())
            : TypedResults.NotFound(result.GetFirstErrorMessage());
    }
}