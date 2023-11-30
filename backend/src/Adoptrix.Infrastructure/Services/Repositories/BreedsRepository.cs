using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Infrastructure.Services.Repositories;

public class BreedsRepository(AdoptrixDbContext dbContext)
    : IBreedsRepository
{
    public async Task<IEnumerable<SearchBreedsResult>> SearchAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Breeds
            .Select(breed => new SearchBreedsResult
            {
                Id = breed.Id,
                Name = breed.Name,
                Species = breed.Species.Name,
                AnimalIds = breed.Animals.Select(animal => animal.Id)
            })
            .OrderBy(result => result.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<Breed>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var breed = await dbContext.Breeds
            .Include(breed => breed.Species)
            .FirstOrDefaultAsync(breed => breed.Name == name, cancellationToken);

        return breed is not null
            ? breed
            : new BreedNotFoundError(name);
    }

    public async Task<Result<Breed>> AddAsync(Breed breed, CancellationToken cancellationToken = default)
    {
        var entry = dbContext.Breeds.Add(breed);
        await dbContext.SaveChangesAsync(cancellationToken);

        return entry.Entity;
    }

    public async Task<Result> DeleteAsync(int breedId, CancellationToken cancellationToken = default)
    {
        var breed = await dbContext.Breeds.FindAsync(new object?[] { breedId }, cancellationToken: cancellationToken);
        if (breed is null)
        {
            return new BreedNotFoundError(breedId);
        }

        dbContext.Breeds.Remove(breed);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}