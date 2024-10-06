using System.Linq.Expressions;
using Adoptrix.Core;
using Adoptrix.Logic.Abstractions;
using Adoptrix.Logic.Errors;
using EntityFramework.Exceptions.Common;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Persistence.Services;

public class BreedsRepository(AdoptrixDbContext dbContext) : IBreedsRepository
{
    public async Task<Breed?> GetAsync(int breedId, CancellationToken cancellationToken = default)
    {
        var breed = await dbContext.Breeds.Where(breed => breed.Id == breedId)
            .Include(breed => breed.Species)
            .FirstOrDefaultAsync(cancellationToken);

        return breed;
    }

    public async Task<TResponse?> GetAsync<TResponse>(int breedId, Expression<Func<Breed, TResponse>> selector,
        CancellationToken cancellationToken = default)
    {
        var response = await dbContext.Breeds
            .AsNoTracking()
            .Where(breed => breed.Id == breedId)
            .Select(selector)
            .FirstOrDefaultAsync(cancellationToken);

        return response;
    }

    public async Task<Result> AddAsync(Breed breed, CancellationToken cancellationToken = default)
    {
        dbContext.Breeds.Add(breed);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        catch (UniqueConstraintException exception) when (exception.ConstraintProperties.Contains(nameof(Breed.Name)))
        {
            return new DuplicateBreedError(breed.Name);
        }
    }

    public async Task<Result> UpdateAsync(Breed breed, CancellationToken cancellationToken = default)
    {
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        catch (UniqueConstraintException exception) when (exception.ConstraintProperties.Contains(nameof(Breed.Name)))
        {
            return new DuplicateBreedError(breed.Name);
        }
    }

    public async Task<Result> DeleteAsync(Breed breed, CancellationToken cancellationToken = default)
    {
        dbContext.Breeds.Remove(breed);
        var changeCount = await dbContext.SaveChangesAsync(cancellationToken);
        return Result.OkIf(changeCount > 0, "Unexpected error deleting breed");
    }
}
