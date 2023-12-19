﻿using Adoptrix.Api.Tests.Generators;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Errors;
using FluentResults;

namespace Adoptrix.Api.Tests.Mocks;

public class MockAnimalsRepository : IAnimalsRepository
{
    public const int UnknownAnimalId = 404;

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
                PrimaryImage = animal.Images.Count > 0 ? animal.Images[0] : null
            });

        return Task.FromResult(animals);
    }

    public Task<Result<Animal>> GetAsync(int animalId, CancellationToken cancellationToken = default)
    {
        if (animalId == UnknownAnimalId)
        {
            var error = new AnimalNotFoundError(UnknownAnimalId);
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

    public Task DeleteAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}