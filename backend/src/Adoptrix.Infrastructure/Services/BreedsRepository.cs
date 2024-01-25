using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Infrastructure.Services;

public sealed class BreedsRepository(AdoptrixDbContext dbContext) : Repository(dbContext), IBreedsRepository
{
    public async Task<IEnumerable<SearchBreedsResult>> SearchAsync(Species? species, bool withAnimals,
        CancellationToken cancellationToken)
    {
        return await DbContext.Breeds
            .Where(breed => (species != null && breed.Species == species || species == null) &&
                            (withAnimals && breed.Animals.Count > 0 || !withAnimals))
            .Select(breed => new SearchBreedsResult
            {
                Id = breed.Id,
                Name = breed.Name,
                SpeciesName = breed.Species.Name,
                AnimalIds = breed.Animals.Select(animal => animal.Id)
            })
            .OrderBy(result => result.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<Breed>> GetByIdAsync(Guid breedId, CancellationToken cancellationToken = default)
    {
        var breed = await DbContext.Breeds
            .Include(breed => breed.Species)
            .Include(breed => breed.Animals)
            .FirstOrDefaultAsync(breed => breed.Id == breedId, cancellationToken);

        return breed is not null
            ? breed
            : new BreedNotFoundError(breedId);
    }

    public async Task<Result<Breed>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var breed = await DbContext.Breeds
            .Include(breed => breed.Species)
            .Include(breed => breed.Animals)
            .FirstOrDefaultAsync(breed => breed.Name == name, cancellationToken);

        return breed is not null
            ? breed
            : new BreedNotFoundError(name);
    }

    public async Task<Result<Breed>> AddAsync(Breed breed, CancellationToken cancellationToken = default)
    {
        var entry = DbContext.Breeds.Add(breed);
        await SaveChangesAsync(cancellationToken);

        return entry.Entity;
    }

    public async Task<Result<Breed>> UpdateAsync(Breed breed, CancellationToken cancellationToken = default)
    {
        var result = await SaveChangesAsync(cancellationToken);
        return result.ToResult(breed);
    }

    public async Task<Result> DeleteAsync(Guid breedId, CancellationToken cancellationToken = default)
    {
        var breed = await DbContext.Breeds.FindAsync([breedId], cancellationToken: cancellationToken);
        if (breed is null)
        {
            return new BreedNotFoundError(breedId);
        }

        DbContext.Breeds.Remove(breed);
        await SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}