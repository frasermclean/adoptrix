using System.Linq.Expressions;
using Adoptrix.Core;
using Adoptrix.Core.Events;
using Adoptrix.Core.Extensions;
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
    Task<IEnumerable<AnimalMatch>> SearchAsync(SearchAnimalsRequest request, CancellationToken cancellationToken);
    Task<Result<AnimalResponse>> GetAsync(Guid animalId, CancellationToken cancellationToken);
    Task<Result<AnimalResponse>> GetAsync(string animalSlug, CancellationToken cancellationToken);
    Task<Result<AnimalResponse>> AddAsync(AddAnimalRequest request, CancellationToken cancellationToken);
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
    public Task<IEnumerable<AnimalMatch>> SearchAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken) => animalsRepository.SearchAsync(request, cancellationToken);

    public async Task<Result<AnimalResponse>> GetAsync(Guid animalId, CancellationToken cancellationToken)
    {
        var response = await animalsRepository.GetProjectionAsync(
            predicate: animal => animal.Id == animalId,
            selector: AnimalResponseSelector,
            cancellationToken);

        return response is null
            ? new AnimalNotFoundError(animalId)
            : response;
    }

    public async Task<Result<AnimalResponse>> GetAsync(string animalSlug, CancellationToken cancellationToken)
    {
        var response = await animalsRepository.GetProjectionAsync(
            predicate: animal => animal.Slug == animalSlug,
            selector: AnimalResponseSelector,
            cancellationToken);

        return response is null
            ? new AnimalNotFoundError(animalSlug)
            : response;
    }

    public async Task<Result<AnimalResponse>> AddAsync(AddAnimalRequest request, CancellationToken cancellationToken)
    {
        var breed = await breedsRepository.GetAsync(request.BreedId, cancellationToken);
        if (breed is null)
        {
            logger.LogError("Breed with ID {BreedId} was not found", request.BreedId);
            return new BreedNotFoundError(request.BreedId);
        }

        var animal = request.ToAnimal(breed);
        await animalsRepository.AddAsync(animal, cancellationToken);

        logger.LogInformation("Animal with ID {AnimalId} was added successfully", animal.Id);
        return animal.ToResponse();
    }

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

    private static readonly Expression<Func<Animal, AnimalResponse>> AnimalResponseSelector = animal =>
        new AnimalResponse
        {
            Id = animal.Id,
            Name = animal.Name,
            Description = animal.Description,
            SpeciesName = animal.Breed.Species.Name,
            BreedName = animal.Breed.Name,
            Sex = animal.Sex,
            DateOfBirth = animal.DateOfBirth,
            Slug = animal.Slug,
            Age = "",
            LastModifiedUtc = animal.LastModifiedUtc,
            Images = animal.Images.Select(image => new AnimalImageResponse
            {
                Id = image.Id,
                Description = image.Description,
                IsProcessed = image.IsProcessed
            })
        };
}
