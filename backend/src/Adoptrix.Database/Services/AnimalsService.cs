using Adoptrix.Application.Contracts.Requests;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Extensions;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Database.Services;

public class AnimalsService(AdoptrixDbContext dbContext) : IAnimalsService
{
    public async Task<IEnumerable<SearchAnimalsResult>> SearchAsync(string? animalName = null,
        Guid? breedId = null, CancellationToken cancellationToken = default)
    {
        return await dbContext.Animals
            .AsNoTracking()
            .Where(animal => (animalName == null || animal.Name.Contains(animalName)) &&
                             (breedId == null || animal.Breed.Id == breedId))
            .Select(animal => new SearchAnimalsResult
            {
                Id = animal.Id,
                Name = animal.Name,
                SpeciesName = animal.Breed.Species.Name,
                BreedName = animal.Breed.Name,
                Sex = animal.Sex,
                DateOfBirth = animal.DateOfBirth,
                CreatedAt = animal.CreatedAt,
                Image = animal.Images.Select(image => new AnimalImageResponse
                    {
                        Id = image.Id, Description = image.Description, IsProcessed = image.IsProcessed
                    })
                    .FirstOrDefault(),
            })
            .OrderBy(animal => animal.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<Animal>> GetAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var animal = await dbContext.Animals.Where(animal => animal.Id == animalId)
            .Include(animal => animal.Breed)
            .ThenInclude(breed => breed.Species)
            .FirstOrDefaultAsync(cancellationToken);

        return animal is null
            ? new AnimalNotFoundError(animalId)
            : animal;
    }

    public async Task<Result<Animal>> AddAsync(SetAnimalRequest request, CancellationToken cancellationToken = default)
    {
        var breed = await dbContext.Breeds.FirstOrDefaultAsync(breed => breed.Id == request.BreedId, cancellationToken);
        if (breed is null)
        {
            return new BreedNotFoundError(request.BreedId);
        }

        var animal = request.ToAnimal(breed);
        var entry = dbContext.Animals.Add(animal);

        await dbContext.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }
    public async Task<Result<Animal>> UpdateAsync(Guid animalId, SetAnimalRequest request,
        CancellationToken cancellationToken = default)
    {
        // get animal from database
        var animal = await dbContext.Animals.FirstOrDefaultAsync(animal => animal.Id == animalId, cancellationToken);
        if (animal is null)
        {
            return new AnimalNotFoundError(animalId);
        }

        // get breed from database
        var breed = await dbContext.Breeds.FirstOrDefaultAsync(breed => breed.Id == request.BreedId, cancellationToken);
        if (breed is null)
        {
            return new BreedNotFoundError(request.BreedId);
        }

        // update properties on the animal
        animal.Name = request.Name;
        animal.Description = request.Description;
        animal.Breed = breed;
        animal.Sex = request.Sex;
        animal.DateOfBirth = request.DateOfBirth;

        await dbContext.SaveChangesAsync(cancellationToken);
        return animal;
    }

    public async Task<Result> DeleteAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var animal = await dbContext.Animals.FirstOrDefaultAsync(animal => animal.Id == animalId, cancellationToken);
        if (animal is null)
        {
            return new AnimalNotFoundError(animalId);
        }

        dbContext.Animals.Remove(animal);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
    public async Task<Result<Animal>> AddImagesAsync(Guid animalId, IEnumerable<AnimalImage> images,
        CancellationToken cancellationToken = default)
    {
        var animal = await dbContext.Animals.FirstOrDefaultAsync(animal => animal.Id == animalId, cancellationToken);
        if (animal is null)
        {
            return new AnimalNotFoundError(animalId);
        }

        animal.Images.AddRange(images);
        await dbContext.SaveChangesAsync(cancellationToken);

        return animal;
    }

    public async Task<Result> SetImageProcessedAsync(Guid animalId, Guid imageId, CancellationToken cancellationToken = default)
    {
        var getResult = await GetAsync(animalId, cancellationToken);
        if (getResult.IsFailed)
        {
            return getResult.ToResult();
        }

        var animal = getResult.Value;

        var image = animal.Images.First(image => image.Id == imageId);
        image.IsProcessed = true;

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }
}
