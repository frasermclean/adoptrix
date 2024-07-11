﻿using Adoptrix.Core.Abstractions;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Extensions;
using Adoptrix.Mapping;
using Adoptrix.Persistence;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Animals;

[HttpGet("animals/{animalId:guid}"), AllowAnonymous]
public class GetAnimalEndpoint(
    IAnimalsRepository animalsRepository,
    [FromKeyedServices(BlobContainerNames.AnimalImages)]
    IBlobContainerManager blobContainerManager)
    : Endpoint<GetAnimalRequest, Results<Ok<AnimalResponse>, NotFound>>
{
    public override async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(GetAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var animal = await animalsRepository.GetByIdAsync(request.AnimalId, cancellationToken);
        if (animal is null)
        {
            return TypedResults.NotFound();
        }

        var response = animal.ToResponse();
        foreach (var image in response.Images)
        {
            image.SetImageUrls(animal.Id, blobContainerManager.ContainerUri);
        }

        return TypedResults.Ok(response);
    }
}
