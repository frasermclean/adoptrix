using Adoptrix.Core.Extensions;
using Adoptrix.Core.Requests;
using Adoptrix.Core.Responses;
using Adoptrix.Logic.Abstractions;
using Adoptrix.Logic.Errors;
using Adoptrix.Logic.Mapping;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Logic.Services;

public interface IBreedsService
{
    Task<IEnumerable<BreedMatch>> SearchAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<BreedResponse>> GetAsync(int breedId, CancellationToken cancellationToken = default);
    Task<Result<BreedResponse>> AddAsync(AddBreedRequest request, CancellationToken cancellationToken = default);
    Task<Result<BreedResponse>> UpdateAsync(UpdateBreedRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int breedId, CancellationToken cancellationToken = default);
}

public class BreedsService(
    ILogger<BreedsService> logger,
    IBreedsRepository breedsRepository,
    ISpeciesRepository speciesRepository) : IBreedsService
{
    public Task<IEnumerable<BreedMatch>> SearchAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken = default) => breedsRepository.SearchAsync(request, cancellationToken);

    public async Task<Result<BreedResponse>> GetAsync(int breedId, CancellationToken cancellationToken = default)
    {
        var response = await breedsRepository.GetAsync(breedId, breed => new BreedResponse
        {
            Id = breed.Id,
            Name = breed.Name,
            SpeciesName = breed.Species.Name
        }, cancellationToken);

        return response is null
            ? new BreedNotFoundError(breedId)
            : response;
    }

    public async Task<Result<BreedResponse>> AddAsync(AddBreedRequest request, CancellationToken cancellationToken)
    {
        // ensure species exists
        var species = await speciesRepository.GetAsync(request.SpeciesName, cancellationToken);
        if (species is null)
        {
            logger.LogError("Species with name {SpeciesName} not found", request.SpeciesName);
            return new SpeciesNotFoundError(request.SpeciesName);
        }

        var breed = request.ToBreed(species);
        var result = await breedsRepository.AddAsync(breed, cancellationToken);

        if (result.IsSuccess)
        {
            logger.LogInformation("Added breed with name {BreedName} and species {SpeciesName}", request.Name,
                request.SpeciesName);

            return breed.ToResponse();
        }

        if (result.HasError<DuplicateBreedError>())
        {
            logger.LogError("Breed with name {BreedName} already exists", request.Name);
        }

        return result;
    }

    public async Task<Result<BreedResponse>> UpdateAsync(UpdateBreedRequest request,
        CancellationToken cancellationToken)
    {
        // ensure breed exists
        var breed = await breedsRepository.GetAsync(request.BreedId, cancellationToken);
        if (breed is null)
        {
            return new BreedNotFoundError(request.BreedId);
        }

        // ensure species exists
        var species = await speciesRepository.GetAsync(request.SpeciesName, cancellationToken);
        if (species is null)
        {
            return new SpeciesNotFoundError(request.SpeciesName);
        }

        breed.Update(request, species);
        var result = await breedsRepository.UpdateAsync(breed, cancellationToken);

        if (result.IsSuccess)
        {
            logger.LogInformation("Updated breed with ID {BreedId}", request.BreedId);
            return breed.ToResponse();
        }

        if (result.HasError<DuplicateBreedError>())
        {
            logger.LogError("Breed with name {BreedName} already exists", request.Name);
        }

        return result;
    }

    public async Task<Result> DeleteAsync(int breedId, CancellationToken cancellationToken)
    {
        var breed = await breedsRepository.GetAsync(breedId, cancellationToken);
        if (breed is null)
        {
            logger.LogError("Could not delete breed with ID {BreedId} because it was not found", breedId);
            return new BreedNotFoundError(breedId);
        }

        var result = await breedsRepository.DeleteAsync(breed, cancellationToken);

        if (result.IsSuccess)
        {
            logger.LogInformation("Breed with ID {BreedId} was deleted", breedId);
            return Result.Ok();
        }

        logger.LogError("Failed to delete breed with ID {BreedId}", breedId);
        return result;
    }
}
