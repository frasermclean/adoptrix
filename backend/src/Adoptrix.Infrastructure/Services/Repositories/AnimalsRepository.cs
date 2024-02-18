using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Infrastructure.Services.Repositories;

public class AnimalsRepository(AdoptrixDbContext dbContext) : Repository(dbContext), IAnimalsRepository
{
    public async Task<IEnumerable<SearchAnimalsResult>> SearchAnimalsAsync(string? animalName = null,
        string? speciesName = null, CancellationToken cancellationToken = default)
    {
        return await DbContext.Animals
            .AsNoTracking()
            .Where(animal => (animalName == null || animal.Name.Contains(animalName)) &&
                             (speciesName == null || animal.Species.Name == speciesName))
            .Select(animal => new SearchAnimalsResult
            {
                Id = animal.Id,
                Name = animal.Name,
                Description = animal.Description,
                SpeciesName = animal.Species.Name,
                BreedName = animal.Breed != null ? animal.Breed.Name : null,
                Sex = animal.Sex,
                DateOfBirth = animal.DateOfBirth,
                CreatedAt = animal.CreatedAt,
                Images = animal.Images,
            })
            .OrderBy(animal => animal.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<Animal>> GetAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var animal = await DbContext.Animals.Where(animal => animal.Id == animalId)
            .Include(animal => animal.Species)
            .Include(animal => animal.Breed)
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

    public async Task<Result<Animal>> UpdateAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
        return animal;
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
