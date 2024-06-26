﻿using Adoptrix.Application.Features.Breeds.Responses;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Database.Services;

public class BreedsRepository(AdoptrixDbContext dbContext, IBatchManager batchManager)
    : Repository(dbContext, batchManager), IBreedsRepository
{
    public async Task<IEnumerable<SearchBreedsResult>> SearchAsync(Guid? speciesId = null, bool? withAnimals = null,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Breeds
            .Where(breed => (speciesId == null || breed.Species.Id == speciesId) &&
                            (withAnimals == null || withAnimals.Value && breed.Animals.Count > 0))
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
    public async Task<Breed?> GetByIdAsync(Guid breedId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Breeds
            .Include(breed => breed.Species)
            .Include(breed => breed.Animals)
            .FirstOrDefaultAsync(breed => breed.Id == breedId, cancellationToken);
    }

    public async Task<Breed?> GetByNameAsync(string breedName, CancellationToken cancellationToken = default)
    {
        return await DbContext.Breeds
            .Include(breed => breed.Species)
            .Include(breed => breed.Animals)
            .FirstOrDefaultAsync(breed => breed.Name == breedName, cancellationToken);
    }

    public async Task AddAsync(Breed breed, CancellationToken cancellationToken = default)
    {
        DbContext.Breeds.Add(breed);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Breed breed, CancellationToken cancellationToken = default)
    {
        DbContext.Breeds.Update(breed);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Breed breed, CancellationToken cancellationToken = default)
    {
        DbContext.Breeds.Remove(breed);
        await SaveChangesAsync(cancellationToken);
    }
}
