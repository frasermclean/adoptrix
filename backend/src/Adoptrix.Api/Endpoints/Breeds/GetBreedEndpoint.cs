﻿using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Contracts.Requests.Breeds;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds;

public static class GetBreedEndpoint
{
    public const string EndpointName = "GetBreed";

    public static async Task<Results<Ok<BreedResponse>, NotFound>> ExecuteAsync(
        string breedIdOrName,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetBreedRequest(breedIdOrName), cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value.ToResponse())
            : TypedResults.NotFound();
    }
}
