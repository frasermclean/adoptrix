using Adoptrix.Api.Tests.Generators;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Errors;
using FluentResults;

namespace Adoptrix.Api.Tests.Mocks;

public class MockAnimalsRepository : IAnimalsRepository
{
    public Task<IEnumerable<SearchAnimalsResult>> SearchAnimalsAsync(string? animalName = null,
        string? speciesName = null, CancellationToken cancellationToken = default)
    {
        var animals = AnimalGenerator.Generate(3)
            .Select(animal => new SearchAnimalsResult()
            {
                Id = animal.Id,
                Name = animal.Name,
                Description = animal.Description,
                Species = animal.Species.Name,
                Breed = animal.Breed?.Name,
                Sex = animal.Sex,
                DateOfBirth = animal.DateOfBirth,
                CreatedAt = animal.CreatedAt,
                Images = animal.Images
            });

        return Task.FromResult(animals);
    }

    public Task<Result<Animal>> GetAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        if (animalId == Guid.Empty)
        {
            var error = new AnimalNotFoundError(Guid.Empty);
            var result = new Result<Animal>().WithError(error);

            return Task.FromResult(result);
        }

        var animal = AnimalGenerator.Generate();
        return Task.FromResult(Result.Ok(animal));
    }

    public Task<Result<Animal>> AddAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Result.Ok(animal));
    }

    public Task<Result<Animal>> UpdateAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}