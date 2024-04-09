using Adoptrix.Application.Errors;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Models;
using FluentResults;

namespace Adoptrix.Application.Services;

public sealed class BreedsService(IBreedsRepository breedsRepository) : IBreedsService
{
    public Task<IEnumerable<SearchBreedsResult>> SearchAsync(Guid? speciesId, bool? withAnimals,
        CancellationToken cancellationToken)
    {
        return breedsRepository.SearchAsync(speciesId, withAnimals, cancellationToken);
    }

    public async Task<Result<Breed>> GetByIdAsync(Guid breedId, CancellationToken cancellationToken = default)
    {
        var breed = await breedsRepository.GetByIdAsync(breedId, cancellationToken);

        return breed is not null
            ? breed
            : new BreedNotFoundError(breedId);
    }

    public async Task<Result<Breed>> GetByNameAsync(string breedName, CancellationToken cancellationToken = default)
    {
        var breed = await breedsRepository.GetByNameAsync(breedName, cancellationToken);

        return breed is not null
            ? breed
            : new BreedNotFoundError(breedName);
    }

    public async Task<Result> AddAsync(Breed breed, CancellationToken cancellationToken = default)
    {
        await breedsRepository.AddAsync(breed, cancellationToken);
        return Result.Ok();
    }

    public async Task<Result<Breed>> UpdateAsync(Breed breed, CancellationToken cancellationToken = default)
    {
        await breedsRepository.UpdateAsync(breed, cancellationToken);
        return breed;
    }

    public async Task<Result> DeleteAsync(Guid breedId, CancellationToken cancellationToken = default)
    {
        var breed = await breedsRepository.GetByIdAsync(breedId, cancellationToken);
        if (breed is null)
        {
            return new BreedNotFoundError(breedId);
        }

        await breedsRepository.DeleteAsync(breed, cancellationToken);
        return Result.Ok();
    }
}
