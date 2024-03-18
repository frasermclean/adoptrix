using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Infrastructure.Services.Repositories;

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
                SpeciesId = breed.Species.Id,
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

    public async Task<Result<Breed>> GetByNameAsync(string breedName, CancellationToken cancellationToken = default)
    {
        var breed = await DbContext.Breeds
            .Include(breed => breed.Species)
            .Include(breed => breed.Animals)
            .FirstOrDefaultAsync(breed => breed.Name == breedName, cancellationToken);

        return breed is not null
            ? breed
            : new BreedNotFoundError(breedName);
    }

    public async Task<Result> AddAsync(Breed breed, CancellationToken cancellationToken = default)
    {
        DbContext.Breeds.Add(breed);
        return await SaveChangesAsync(cancellationToken);
    }

    public async Task<Result<Breed>> UpdateAsync(Breed breed, CancellationToken cancellationToken = default)
    {
        var result = await SaveChangesAsync(cancellationToken);
        return result.ToResult(breed);
    }

    public async Task<Result> DeleteAsync(Guid breedId, CancellationToken cancellationToken = default)
    {
        var getResult = await GetByIdAsync(breedId, cancellationToken);
        if (getResult.IsFailed)
        {
            return getResult.ToResult();
        }

        DbContext.Breeds.Remove(getResult.Value);
        return await SaveChangesAsync(cancellationToken);
    }
}
