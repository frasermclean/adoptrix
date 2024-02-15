using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Events;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Services;

public interface IAnimalsService
{
    Task<Result<Animal>> GetAnimalByIdAsync(Guid animalId, CancellationToken cancellationToken);
    Task<Result<Animal>> AddAnimalAsync(string name, string? description, string speciesName, string? breedName,
        Sex? sex, DateOnly dateOfBirth, Guid createdBy, CancellationToken cancellationToken = default);
    Task<Result> DeleteAnimalAsync(Guid animalId, CancellationToken cancellationToken);
}

public class AnimalsService(
    ILogger<AnimalsService> logger,
    IAnimalsRepository repository,
    ISpeciesService speciesService,
    IBreedsService breedsService,
    IEventPublisher eventPublisher)
    : IAnimalsService
{
    public async Task<Result<Animal>> GetAnimalByIdAsync(Guid animalId, CancellationToken cancellationToken)
    {
        return await repository.GetAsync(animalId, cancellationToken);
    }

    public async Task<Result<Animal>> AddAnimalAsync(string name, string? description, string speciesName,
        string? breedName, Sex? sex, DateOnly dateOfBirth, Guid createdBy, CancellationToken cancellationToken)
    {
        // find species
        var speciesResult = await speciesService.GetByNameAsync(speciesName, cancellationToken);
        if (speciesResult.IsFailed)
        {
            return speciesResult.ToResult();
        }

        // find breed if breed name was specified
        var breedResult = breedName is not null
            ? await breedsService.GetByNameAsync(breedName, cancellationToken)
            : null;
        if (breedResult?.IsFailed ?? false)
        {
            return breedResult.ToResult();
        }

        var animal = new Animal
        {
            Name = name,
            Description = description,
            Species = speciesResult.Value,
            Breed = breedResult?.Value,
            Sex = sex,
            DateOfBirth = dateOfBirth,
            CreatedBy = createdBy
        };

        var addResult = await repository.AddAsync(animal, cancellationToken);

        if (addResult.IsFailed)
        {
            logger.LogError("Could not add animal with name {AnimalName}", name);
        }

        return addResult;
    }
    public async Task<Result> DeleteAnimalAsync(Guid animalId, CancellationToken cancellationToken)
    {
        // find existing animal
        var getResult = await GetAnimalByIdAsync(animalId, cancellationToken);
        if (getResult.IsFailed)
        {
            logger.LogError("Could not find animal with ID: {AnimalId}", animalId);
            return getResult.ToResult();
        }

        // delete animal from database
        var animal = getResult.Value;
        var deleteResult = await repository.DeleteAsync(animal, cancellationToken);
        if (deleteResult.IsFailed)
        {
            logger.LogError("Could not delete animal with ID: {AnimalId}", animalId);
            return deleteResult;
        }

        // publish domain event
        var domainEvent = new AnimalDeletedEvent(animal.Id);
        await eventPublisher.PublishDomainEventAsync(domainEvent, cancellationToken);

        return Result.Ok();
    }
}
