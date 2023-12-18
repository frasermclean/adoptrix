using Adoptrix.Api.Tests.Generators;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Errors;
using FluentResults;

namespace Adoptrix.Api.Tests.Mocks;

public class MockBreedsRepository : IBreedsRepository
{
    public const string UnknownBreedName = "unknown";

    public Task<IEnumerable<SearchBreedsResult>> SearchAsync(Species? species = null, bool withAnimals = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Breed>> GetByIdAsync(int breedId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Breed>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (name == UnknownBreedName)
        {
            var error = new BreedNotFoundError(UnknownBreedName);
            var result = new Result<Breed>().WithError(error);

            return Task.FromResult(result);
        }

        var breed = BreedGenerator.Generate();
        return Task.FromResult(Result.Ok(breed));
    }

    public Task<Result<Breed>> AddAsync(Breed breed, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Breed>> UpdateAsync(Breed breed, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(int breedId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}