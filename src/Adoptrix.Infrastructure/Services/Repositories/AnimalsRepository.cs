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
    public async Task<IEnumerable<AnimalSearchResult>> SearchAsync(string? name = null, Species? species = null,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Animals
            .Where(animal => (name == null || animal.Name.Contains(name)) &&
                             (species == null || animal.Species == species))
            .Select(animal => AnimalSearchResult.FromAnimal(animal))
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<Animal>> GetAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var animal = await dbContext.Animals.FindAsync(new object?[] { animalId }, cancellationToken);

        return animal is null
            ? new AnimalNotFoundError(animalId)
            : animal;
    }

    public async Task<Result<Animal>> AddAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        var entry = await dbContext.Animals.AddAsync(animal, cancellationToken);
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