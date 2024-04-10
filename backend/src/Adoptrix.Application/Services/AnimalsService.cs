using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Extensions;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Models;
using FluentResults;

namespace Adoptrix.Application.Services;

public interface IAnimalsService
{
    Task<Result<Animal>> GetAsync(Guid animalId, CancellationToken cancellationToken = default);
    Task<Result<Animal>> AddAsync(SetAnimalRequest request, CancellationToken cancellationToken = default);

    Task<Result<Animal>> UpdateAsync(Guid animalId, SetAnimalRequest request,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(Guid animalId, CancellationToken cancellationToken = default);

    Task<Result<Animal>> AddImagesAsync(Guid animalId, IEnumerable<AnimalImage> images,
        CancellationToken cancellationToken = default);

    Task<Result> SetImageProcessedAsync(Guid animalId, Guid imageId, CancellationToken cancellationToken = default);
}

public class AnimalsService(IAnimalsRepository animalsRepository, IBreedsRepository breedsRepository) : IAnimalsService
{
    public async Task<Result<Animal>> GetAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var animal = await animalsRepository.GetByIdAsync(animalId, cancellationToken);

        return animal is not null
            ? animal
            : new AnimalNotFoundError(animalId);
    }

    public async Task<Result<Animal>> AddAsync(SetAnimalRequest request, CancellationToken cancellationToken = default)
    {
        var breed = await breedsRepository.GetByIdAsync(request.BreedId, cancellationToken);
        if (breed is null)
        {
            return new BreedNotFoundError(request.BreedId);
        }

        var animal = request.ToAnimal(breed);
        await animalsRepository.AddAsync(animal, cancellationToken);

        return animal;
    }

    public async Task<Result<Animal>> UpdateAsync(Guid animalId, SetAnimalRequest request,
        CancellationToken cancellationToken = default)
    {
        var animal = await animalsRepository.GetByIdAsync(animalId, cancellationToken);
        if (animal is null)
        {
            return new AnimalNotFoundError(animalId);
        }

        var breed = await breedsRepository.GetByIdAsync(request.BreedId, cancellationToken);
        if (breed is null)
        {
            return new BreedNotFoundError(request.BreedId);
        }

        animal.Name = request.Name;
        animal.Description = request.Description;
        animal.Breed = breed;
        animal.Sex = request.Sex;
        animal.DateOfBirth = request.DateOfBirth;

        await animalsRepository.UpdateAsync(animal, cancellationToken);
        return animal;
    }

    public async Task<Result> DeleteAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var animal = await animalsRepository.GetByIdAsync(animalId, cancellationToken);
        if (animal is null)
        {
            return new AnimalNotFoundError(animalId);
        }

        await animalsRepository.DeleteAsync(animal, cancellationToken);

        return Result.Ok();
    }

    public async Task<Result<Animal>> AddImagesAsync(Guid animalId, IEnumerable<AnimalImage> images,
        CancellationToken cancellationToken = default)
    {
        var animal = await animalsRepository.GetByIdAsync(animalId, cancellationToken);
        if (animal is null)
        {
            return new AnimalNotFoundError(animalId);
        }

        animal.Images.AddRange(images);
        await animalsRepository.UpdateAsync(animal, cancellationToken);

        return animal;
    }

    public async Task<Result> SetImageProcessedAsync(Guid animalId, Guid imageId,
        CancellationToken cancellationToken = default)
    {
        var animal = await animalsRepository.GetByIdAsync(animalId, cancellationToken);
        if (animal is null)
        {
            return new AnimalNotFoundError(animalId);
        }

        var image = animal.Images.First(image => image.Id == imageId);
        image.IsProcessed = true;

        await animalsRepository.UpdateAsync(animal, cancellationToken);
        return Result.Ok();
    }
}
