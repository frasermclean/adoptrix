using Adoptrix.Core;
using Adoptrix.Core.Abstractions;
using Adoptrix.Core.Contracts.Requests.Breeds;
using Adoptrix.Core.Contracts.Responses;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Database.Services;

public class BreedsRepository(AdoptrixDbContext dbContext) : IBreedsRepository
{
    public async Task<IEnumerable<BreedMatch>> SearchAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Breeds
            .Where(breed => (request.SpeciesId == null || breed.Species.Id == request.SpeciesId) &&
                            (request.WithAnimals == null || request.WithAnimals.Value && breed.Animals.Count > 0))
            .Select(breed => new BreedMatch
            {
                Id = breed.Id,
                Name = breed.Name,
                SpeciesId = breed.Species.Id,
                AnimalCount = breed.Animals.Count(animal => animal.Breed.Id == breed.Id)
            })
            .OrderBy(result => result.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Breed?> GetByIdAsync(Guid breedId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Breeds
            .Include(breed => breed.Species)
            .Include(breed => breed.Animals)
            .FirstOrDefaultAsync(breed => breed.Id == breedId, cancellationToken);
    }

    public async Task<Breed?> GetByNameAsync(string breedName, CancellationToken cancellationToken = default)
    {
        return await dbContext.Breeds
            .Include(breed => breed.Species)
            .Include(breed => breed.Animals)
            .FirstOrDefaultAsync(breed => breed.Name == breedName, cancellationToken);
    }

    public async Task AddAsync(Breed breed, CancellationToken cancellationToken = default)
    {
        dbContext.Breeds.Add(breed);
        await SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);

    public async Task DeleteAsync(Breed breed, CancellationToken cancellationToken = default)
    {
        dbContext.Breeds.Remove(breed);
        await SaveChangesAsync(cancellationToken);
    }
}
