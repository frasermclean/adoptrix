using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Adoptrix.Logic.Errors;
using Adoptrix.Logic.Mapping;
using Adoptrix.Persistence.Services;
using EntityFramework.Exceptions.Common;
using FluentResults;
using Microsoft.EntityFrameworkCore;
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

public class BreedsService(ILogger<BreedsService> logger, AdoptrixDbContext dbContext) : IBreedsService
{
    public async Task<IEnumerable<BreedMatch>> SearchAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken)
    {
        var matches = await dbContext.Breeds
            .AsNoTracking()
            .Where(breed => (request.SpeciesName == null || breed.Species.Name == request.SpeciesName) &&
                            (request.WithAnimals == null || request.WithAnimals.Value && breed.Animals.Count > 0))
            .OrderBy(breed => breed.Name)
            .Select(breed => new BreedMatch
            {
                Id = breed.Id,
                Name = breed.Name,
                SpeciesName = breed.Species.Name,
                AnimalCount = breed.Animals.Count(animal => animal.Breed.Id == breed.Id)
            })
            .ToListAsync(cancellationToken);

        return matches;
    }

    public async Task<Result<BreedResponse>> GetAsync(int breedId, CancellationToken cancellationToken)
    {
        var response = await dbContext.Breeds
            .AsNoTracking()
            .Where(breed => breed.Id == breedId)
            .Select(breed => new BreedResponse
            {
                Id = breed.Id,
                Name = breed.Name,
                SpeciesName = breed.Species.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        return response is null
            ? new BreedNotFoundError(breedId)
            : response;
    }

    public async Task<Result<BreedResponse>> AddAsync(AddBreedRequest request, CancellationToken cancellationToken)
    {
        var species = await dbContext.Species.Where(species => species.Name == request.SpeciesName)
            .Include(species => species.Breeds)
            .FirstOrDefaultAsync(cancellationToken);

        // ensure species exists
        if (species is null)
        {
            logger.LogError("Species with name {SpeciesName} not found", request.SpeciesName);
            return new SpeciesNotFoundError(request.SpeciesName);
        }

        var breed = new Breed
        {
            Name = request.Name,
            Species = species,
            LastModifiedBy = request.UserId
        };
        species.Breeds.Add(breed);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Added breed with name {BreedName} and species {SpeciesName}", request.Name,
                request.SpeciesName);

            return breed.ToResponse();
        }
        catch (UniqueConstraintException exception) when (exception.ConstraintProperties.Contains(nameof(Breed.Name)))
        {
            logger.LogError(exception, "Breed with name {BreedName} already exists", request.Name);
            return new DuplicateBreedError(request.Name);
        }
    }

    public async Task<Result<BreedResponse>> UpdateAsync(UpdateBreedRequest request,
        CancellationToken cancellationToken)
    {
        // ensure breed exists
        var breed = await dbContext.Breeds.FirstOrDefaultAsync(breed => breed.Id == request.BreedId, cancellationToken);
        if (breed is null)
        {
            return new BreedNotFoundError(request.BreedId);
        }

        // ensure species exists
        var species = await dbContext.Species.FirstOrDefaultAsync(species => species.Name == request.SpeciesName,
            cancellationToken);
        if (species is null)
        {
            return new SpeciesNotFoundError(request.SpeciesName);
        }

        // update breed
        breed.Name = request.Name;
        breed.Species = species;
        breed.LastModifiedBy = request.UserId;
        breed.LastModifiedUtc = DateTime.UtcNow;

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Updated breed with ID {BreedId}", request.BreedId);
            return breed.ToResponse();
        }
        catch (UniqueConstraintException exception) when (exception.ConstraintProperties.Contains(nameof(Breed.Name)))
        {
            logger.LogError(exception, "Breed with name {BreedName} already exists", request.Name);
            return new DuplicateBreedError(request.Name);
        }
    }

    public async Task<Result> DeleteAsync(int breedId, CancellationToken cancellationToken)
    {
        var breed = await dbContext.Breeds.FirstOrDefaultAsync(breed => breed.Id == breedId, cancellationToken);
        if (breed is null)
        {
            logger.LogError("Could not delete breed with ID {BreedId} because it was not found", breedId);
            return new BreedNotFoundError(breedId);
        }

        dbContext.Breeds.Remove(breed);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Breed with ID {BreedId} was deleted", breedId);

        return Result.Ok();
    }
}
