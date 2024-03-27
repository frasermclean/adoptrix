using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Infrastructure.Services.Repositories;

public class AnimalsRepository(AdoptrixDbContext dbContext) : Repository(dbContext), IAnimalsRepository
{
    public async Task<IEnumerable<SearchAnimalsResult>> SearchAsync(string? animalName = null,
        Guid? breedId = null, CancellationToken cancellationToken = default)
    {
        return await DbContext.Animals
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
                Image = animal.Images.Select(image => new ImageResponse
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
        var animal = await DbContext.Animals.Where(animal => animal.Id == animalId)
            .Include(animal => animal.Breed)
            .Include(animal => animal.Breed.Species)
            .FirstOrDefaultAsync(cancellationToken);

        return animal is null
            ? new AnimalNotFoundError(animalId)
            : animal;
    }

    public async Task<Result<Animal>> AddAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        var entry = DbContext.Animals.Add(animal);
        await SaveChangesAsync(cancellationToken);

        return entry.Entity;
    }

    public async Task<Result> UpdateAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> DeleteAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var getResult = await GetAsync(animalId, cancellationToken);
        if (getResult.IsFailed)
        {
            return getResult.ToResult();
        }

        var animal = getResult.Value;
        DbContext.Animals.Remove(animal);
        return await SaveChangesAsync(cancellationToken);
    }
}
