using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Commands.Breeds;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds.AddBreed;

public class AddBreedEndpoint : Endpoint<AddBreedCommand, Results<Created<BreedResponse>, BadRequest<string>>>
{
    public override void Configure()
    {
        Post("admin/breeds");
    }

    public override async Task<Results<Created<BreedResponse>, BadRequest<string>>> ExecuteAsync(
        AddBreedCommand command, CancellationToken cancellationToken)
    {
        var result = await command.ExecuteAsync(cancellationToken);

        if (result.IsFailed)
        {
            return TypedResults.BadRequest(result.GetFirstErrorMessage());
        }

        var response = result.Value.ToResponse();
        return TypedResults.Created($"breeds/{response.Id}", response);
    }
}