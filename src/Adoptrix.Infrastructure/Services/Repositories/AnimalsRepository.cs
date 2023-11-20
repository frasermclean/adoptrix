using Adoptrix.Application.Errors;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Infrastructure.Services.Repositories;

public class AnimalsRepository(AdoptrixDbContext dbContext)
    : IAnimalsRepository
{
    public async Task<IEnumerable<Animal>> SearchAsync(string? name = null, Species? species = null,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Animals
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<Animal>> GetAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var animal = await dbContext.Animals.FirstOrDefaultAsync(a => a.Id == animalId, cancellationToken);

        return animal is null
            ? CreateNotFoundError(animalId)
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
        var existingAnimal = await dbContext.Animals.FirstOrDefaultAsync(a => a.Id == animal.Id, cancellationToken);

        if (existingAnimal is null)
        {
            return CreateNotFoundError(animal.Id);
        }

        // update properties
        existingAnimal.Name = animal.Name;
        existingAnimal.Species = animal.Species;
        existingAnimal.DateOfBirth = animal.DateOfBirth;

        await dbContext.SaveChangesAsync(cancellationToken);
        return existingAnimal;
    }

    public async Task<Result> DeleteAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var animal = await dbContext.Animals.FirstOrDefaultAsync(a => a.Id == animalId, cancellationToken);
        if (animal is null)
        {
            return CreateNotFoundError(animalId);
        }

        dbContext.Animals.Remove(animal);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    private static NotFoundError CreateNotFoundError(Guid animalId)
        => new($"Could not find animal with ID {animalId}");
}