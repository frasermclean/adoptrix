using Adoptrix.Application.Contracts.Requests.Breeds;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Models;
using FluentResults;

namespace Adoptrix.Application.Services;

public interface IBreedsService
{
    Task<IEnumerable<SearchBreedsResult>> SearchAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<Breed>> GetByIdAsync(Guid breedId, CancellationToken cancellationToken = default);
    Task<Result<Breed>> GetByNameAsync(string breedName, CancellationToken cancellationToken = default);
    Task<Result<Breed>> AddAsync(SetBreedRequest request, CancellationToken cancellationToken = default);
    Task<Result<Breed>> UpdateAsync(Guid breedId, SetBreedRequest request,
        CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid breedId, CancellationToken cancellationToken = default);
}

public sealed class BreedsService(IBreedsRepository breedsRepository, ISpeciesRepository speciesRepository)
    : IBreedsService
{
    public Task<IEnumerable<SearchBreedsResult>> SearchAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken)
    {
        return breedsRepository.SearchAsync(request.SpeciesId, request.WithAnimals, cancellationToken);
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

    public async Task<Result<Breed>> AddAsync(SetBreedRequest request, CancellationToken cancellationToken = default)
    {
        var species = await speciesRepository.GetByIdAsync(request.SpeciesId, cancellationToken);
        if (species is null)
        {
            return new SpeciesNotFoundError(request.SpeciesId);
        }

        var breed = new Breed
        {
            Name = request.Name, Species = species, CreatedBy = request.UserId
        };
        await breedsRepository.AddAsync(breed, cancellationToken);

        return breed;
    }

    public async Task<Result<Breed>> UpdateAsync(Guid breedId, SetBreedRequest request,
        CancellationToken cancellationToken = default)
    {
        var breed = await breedsRepository.GetByIdAsync(breedId, cancellationToken);
        if (breed is null)
        {
            return new BreedNotFoundError(breedId);
        }

        var species = await speciesRepository.GetByIdAsync(request.SpeciesId, cancellationToken);
        if (species is null)
        {
            return new SpeciesNotFoundError(request.SpeciesId);
        }

        breed.Name = request.Name;
        breed.Species = species;
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
