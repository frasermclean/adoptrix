﻿using Adoptrix.Api.Mapping;
using Adoptrix.Contracts.Responses;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Breeds;

[HttpGet("breeds/{breedId:guid}"), AllowAnonymous]
public class GetBreedEndpoint(IBreedsRepository breedsRepository)
    : Endpoint<GetBreedRequest, Results<Ok<BreedResponse>, NotFound>>
{
    public override async Task<Results<Ok<BreedResponse>, NotFound>> ExecuteAsync(GetBreedRequest request,
        CancellationToken cancellationToken)
    {
        var breed = await breedsRepository.GetByIdAsync(request.BreedId, cancellationToken);

        return breed is not null
            ? TypedResults.Ok(breed.ToResponse())
            : TypedResults.NotFound();
    }
}
