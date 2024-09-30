using Adoptrix.Core.Events;
using Adoptrix.Core.Extensions;
using Adoptrix.Core.Requests;
using Adoptrix.Core.Responses;
using Adoptrix.Logic.Errors;
using Adoptrix.Logic.Mapping;
using Adoptrix.Persistence.Services;
using FluentResults;
using Microsoft.EntityFrameworkCore;
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

public class AnimalsService(ILogger<AnimalsService> logger, AdoptrixDbContext dbContext, IEventPublisher eventPublisher)
    : IAnimalsService
{
    public async Task<IEnumerable<AnimalMatch>> SearchAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken)
    {
        var matches = await dbContext.Animals
            .AsNoTracking()
            .Where(animal => (request.Name == null || animal.Name.Contains(request.Name)) &&
                             (request.BreedId == null || animal.Breed.Id == request.BreedId) &&
                             (request.SpeciesName == null || animal.Breed.Species.Name == request.SpeciesName) &&
                             (request.Sex == null || animal.Sex == request.Sex))
            .OrderBy(animal => animal.Name)
            .Take(request.Limit ?? 10)
            .Select(animal => new AnimalMatch
            {
                Id = animal.Id,
                Name = animal.Name,
                SpeciesName = animal.Breed.Species.Name,
                BreedName = animal.Breed.Name,
                Slug = animal.Slug,
                Image = animal.Images.Select(image => image.ToResponse())
                    .FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        return matches;
    }

    public async Task<Result<AnimalResponse>> GetAsync(Guid animalId, CancellationToken cancellationToken)
    {
        var response = await dbContext.Animals.Where(animal => animal.Id == animalId)
            .AsNoTracking()
            .Include(animal => animal.Breed)
            .ThenInclude(breed => breed.Species)
            .Select(animal => animal.ToResponse())
            .FirstOrDefaultAsync(cancellationToken);

        return response is null
            ? new AnimalNotFoundError(animalId)
            : response;
    }

    public async Task<Result<AnimalResponse>> GetAsync(string animalSlug, CancellationToken cancellationToken)
    {
        var response = await dbContext.Animals.Where(animal => animal.Slug == animalSlug)
            .AsNoTracking()
            .Include(animal => animal.Breed)
            .ThenInclude(breed => breed.Species)
            .Select(animal => animal.ToResponse())
            .FirstOrDefaultAsync(cancellationToken);

        return response is null
            ? new AnimalNotFoundError(animalSlug)
            : response;
    }

    public async Task<Result<AnimalResponse>> AddAsync(AddAnimalRequest request, CancellationToken cancellationToken)
    {
        var breed = await dbContext.Breeds.Where(breed => breed.Id == request.BreedId)
            .Include(breed => breed.Species)
            .FirstOrDefaultAsync(breed => breed.Id == request.BreedId, cancellationToken);

        if (breed is null)
        {
            logger.LogError("Breed with ID {BreedId} was not found", request.BreedId);
            return new BreedNotFoundError(request.BreedId);
        }

        var animal = request.ToAnimal(breed);
        breed.Animals.Add(animal);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Animal with ID {AnimalId} was added successfully", animal.Id);

        return animal.ToResponse();
    }

    public async Task<Result<AnimalResponse>> UpdateAsync(UpdateAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var animal = await dbContext.Animals.FirstOrDefaultAsync(a => a.Id == request.AnimalId, cancellationToken);
        if (animal is null)
        {
            logger.LogError("Animal with ID {AnimalId} was not found", request.AnimalId);
            return new AnimalNotFoundError(request.AnimalId);
        }

        var breed = await dbContext.Breeds.Where(breed => breed.Id == request.BreedId)
            .Include(breed => breed.Species)
            .FirstOrDefaultAsync(cancellationToken);
        if (breed is null)
        {
            logger.LogError("Breed with ID {BreedId} was not found", request.BreedId);
            return new BreedNotFoundError(request.BreedId);
        }

        animal.Update(request, breed);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Updated animal with ID: {AnimalId}", animal.Id);

        return animal.ToResponse();
    }

    public async Task<Result> DeleteAsync(DeleteAnimalRequest request, CancellationToken cancellationToken)
    {
        var animal =
            await dbContext.Animals.FirstOrDefaultAsync(animal => animal.Id == request.AnimalId, cancellationToken);
        if (animal is null)
        {
            logger.LogError("Could not find animal with ID: {AnimalId} to delete", request.AnimalId);
            return new AnimalNotFoundError(request.AnimalId);
        }

        animal.Delete(request.UserId);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Deleted animal with ID: {AnimalId}", request.AnimalId);

        await eventPublisher.PublishAsync(new AnimalDeletedEvent(animal.Slug), cancellationToken);

        return Result.Ok();
    }
}
