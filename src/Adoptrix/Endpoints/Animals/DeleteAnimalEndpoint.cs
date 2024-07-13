﻿using Adoptrix.Core.Abstractions;
using Adoptrix.Core.Events;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Animals;

[HttpDelete("animals/{animalId:guid}")]
public class DeleteAnimalEndpoint(IAnimalsRepository animalsRepository, IEventPublisher eventPublisher)
    : Endpoint<DeleteAnimalRequest, Results<NoContent, NotFound>>
{
    public override async Task<Results<NoContent, NotFound>> ExecuteAsync(DeleteAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var animal = await animalsRepository.GetByIdAsync(request.AnimalId, cancellationToken);
        if (animal is null)
        {
            Logger.LogError("Could not find animal with ID: {AnimalId} to delete", request.AnimalId);
            return TypedResults.NotFound();
        }

        await animalsRepository.DeleteAsync(animal, cancellationToken);
        Logger.LogInformation("Deleted animal with ID: {AnimalId}", request.AnimalId);

        await eventPublisher.PublishAsync(new AnimalDeletedEvent(request.AnimalId), cancellationToken);

        return TypedResults.NoContent();
    }
}