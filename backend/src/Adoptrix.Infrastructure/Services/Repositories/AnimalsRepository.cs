using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Infrastructure.Services.Repositories;

public class AnimalsRepository(AdoptrixDbContext dbContext)
    : IAnimalsRepository
{
    public async Task<IEnumerable<SearchAnimalsResult>> SearchAnimalsAsync(string? animalName = null,
        string? speciesName = null, CancellationToken cancellationToken = default)
    {
        return await dbContext.Animals
            .AsNoTracking()
            .Where(animal => (animalName == null || animal.Name.Contains(animalName)) &&
                             (speciesName == null || animal.Species.Name == speciesName))
            .Select(animal => new SearchAnimalsResult
            {
                Id = animal.Id,
                Name = animal.Name,
                Description = animal.Description,
                Species = animal.Species.Name,
                Breed = animal.Breed != null ? animal.Breed.Name : null,
                DateOfBirth = animal.DateOfBirth,
                PrimaryImage = animal.Images.Count > 0 ? animal.Images[0] : null
            })
            .OrderBy(animal => animal.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<Animal>> GetAsync(int animalId, CancellationToken cancellationToken = default)
    {
        var animal = await dbContext.Animals.Where(animal => animal.Id == animalId)
            .Include(animal => animal.Species)
            .Include(animal => animal.Breed)
            .FirstOrDefaultAsync(cancellationToken);

        return animal is null
            ? new AnimalNotFoundError(animalId)
            : animal;
    }

    public async Task<Result<Animal>> AddAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        var entry = dbContext.Animals.Add(animal);
        await dbContext.SaveChangesAsync(cancellationToken);

        return entry.Entity;
    }

    public async Task<Result<Animal>> UpdateAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
        return animal;
    }

    public async Task DeleteAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        dbContext.Animals.Remove(animal);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}