﻿using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Services.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public static class GetAnimalEndpoint
{
    public const string EndpointName = "GetAnimal";

    public static async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(Guid animalId,
        IAnimalsRepository animalsRepository, CancellationToken cancellationToken)
    {
        var result = await animalsRepository.GetAsync(animalId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value.ToResponse())
            : TypedResults.NotFound();
    }
}
