using Adoptrix.Application.Mapping;
using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Core;
using Adoptrix.Core.Contracts.Requests.Breeds;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Core.Errors;
using Adoptrix.Core.Services;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Services;

public class BreedsService(
    ILogger<BreedsService> logger,
    IBreedsRepository breedsRepository,
    ISpeciesRepository speciesRepository) : IBreedsService
{
    public Task<IEnumerable<BreedMatch>> SearchAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken = default)
    {
        return breedsRepository.SearchAsync(request.SpeciesId, request.WithAnimals, cancellationToken);
    }

    public async Task<Result<BreedResponse>> GetAsync(Guid breedId, CancellationToken cancellationToken = default)
    {
        var breed = await breedsRepository.GetByIdAsync(breedId, cancellationToken);

        return breed is not null
            ? breed.ToResponse()
            : new BreedNotFoundError(breedId);
    }

    public async Task<Result<BreedResponse>> AddAsync(AddBreedRequest request,
        CancellationToken cancellationToken = default)
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


        return breed.ToResponse();
    }

    public async Task<Result<BreedResponse>> UpdateAsync(UpdateBreedRequest request, CancellationToken cancellationToken = default)
    {
        var breed = await breedsRepository.GetByIdAsync(request.BreedId, cancellationToken);
        if (breed is null)
        {
            return new BreedNotFoundError(request.BreedId);
        }

        var species = await speciesRepository.GetByIdAsync(request.SpeciesId, cancellationToken);
        if (species is null)
        {
            return new SpeciesNotFoundError(request.SpeciesId);
        }

        breed.Name = request.Name;
        breed.Species = species;
        await breedsRepository.UpdateAsync(breed, cancellationToken);

        return breed.ToResponse();
    }

    public async Task<Result> DeleteAsync(Guid breedId, CancellationToken cancellationToken = default)
    {
        var breed = await breedsRepository.GetByIdAsync(breedId, cancellationToken);
        if (breed is null)
        {
            logger.LogError("Could not delete breed with ID {BreedId} because it was not found", breedId);
            return new BreedNotFoundError(breedId);
        }

        await breedsRepository.DeleteAsync(breed, cancellationToken);
        logger.LogInformation("Breed with ID {BreedId} was deleted", breedId);

        return Result.Ok();
    }
}
