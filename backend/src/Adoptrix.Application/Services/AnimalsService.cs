using Adoptrix.Application.Errors;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Models;
using FluentResults;

namespace Adoptrix.Application.Services;

public interface IAnimalsService
{
    Task<Result<Animal>> GetAsync(Guid animalId, CancellationToken cancellationToken = default);

    Task<Result> SetImageProcessedAsync(Guid animalId, Guid imageId, CancellationToken cancellationToken = default);
}

public class AnimalsService(IAnimalsRepository animalsRepository) : IAnimalsService
{
    public async Task<Result<Animal>> GetAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var animal = await animalsRepository.GetByIdAsync(animalId, cancellationToken);

        return animal is not null
            ? animal
            : new AnimalNotFoundError(animalId);
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
