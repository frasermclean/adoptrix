﻿using Adoptrix.Core.Events;
using Adoptrix.Core.Requests;
using Adoptrix.Core.Responses;
using Adoptrix.Logic.Abstractions;
using Adoptrix.Logic.Errors;
using Adoptrix.Logic.Mapping;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Logic.Services;

public interface IAnimalsService
{
    Task<Result<AnimalResponse>> UpdateAsync(UpdateAnimalRequest request, CancellationToken cancellationToken);
    Task<Result> DeleteAsync(DeleteAnimalRequest request, CancellationToken cancellationToken);
}

public class AnimalsService(
    ILogger<AnimalsService> logger,
    IAnimalsRepository animalsRepository,
    IBreedsRepository breedsRepository,
    IEventPublisher eventPublisher)
    : IAnimalsService
{
    public async Task<Result<AnimalResponse>> UpdateAsync(UpdateAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var animal = await animalsRepository.GetAsync(request.AnimalId, cancellationToken);
        if (animal is null)
        {
            logger.LogError("Animal with ID {AnimalId} was not found", request.AnimalId);
            return new AnimalNotFoundError(request.AnimalId);
        }

        var breed = await breedsRepository.GetAsync(request.BreedId, cancellationToken);
        if (breed is null)
        {
            logger.LogError("Breed with ID {BreedId} was not found", request.BreedId);
            return new BreedNotFoundError(request.BreedId);
        }

        animal.Update(request, breed);
        await animalsRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Updated animal with ID: {AnimalId}", animal.Id);

        return animal.ToResponse();
    }

    public async Task<Result> DeleteAsync(DeleteAnimalRequest request, CancellationToken cancellationToken)
    {
        var animal = await animalsRepository.GetAsync(request.AnimalId, cancellationToken);
        if (animal is null)
        {
            logger.LogError("Could not find animal with ID: {AnimalId} to delete", request.AnimalId);
            return new AnimalNotFoundError(request.AnimalId);
        }

        animal.Delete(request.UserId);
        await animalsRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Deleted animal with ID: {AnimalId}", request.AnimalId);
        await eventPublisher.PublishAsync(new AnimalDeletedEvent(animal.Slug), cancellationToken);

        return Result.Ok();
    }
}
