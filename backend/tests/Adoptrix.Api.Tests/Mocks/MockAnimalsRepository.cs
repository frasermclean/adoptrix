using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Bogus;
using FluentResults;

namespace Adoptrix.Api.Tests.Mocks;

public class MockAnimalsRepository : IAnimalsRepository
{
    public Task<IEnumerable<SearchAnimalsResult>> SearchAnimalsAsync(string? animalName = null,
        string? speciesName = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Animal>> GetAsync(int animalId, CancellationToken cancellationToken = default)
    {
        var animal = AnimalGenerator.Generate();
        return Task.FromResult(Result.Ok(animal));
    }

    public Task<Result<Animal>> AddAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Animal>> UpdateAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}