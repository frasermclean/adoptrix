using Adoptrix.Core;
using Adoptrix.Persistence.Responses;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Persistence.Services;

public interface IBreedsRepository
{
    Task<IEnumerable<SearchBreedsItem>> SearchAsync(string? speciesName = null, bool? withAnimals = null,
        CancellationToken cancellationToken = default);

    Task<Breed?> GetByIdAsync(int breedId, CancellationToken cancellationToken = default);
    Task<Breed?> GetByNameAsync(string breedName, CancellationToken cancellationToken = default);
    Task AddAsync(Breed breed, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task DeleteAsync(Breed breed, CancellationToken cancellationToken = default);
}

public class BreedsRepository(AdoptrixDbContext dbContext) : IBreedsRepository
{
    public async Task<IEnumerable<SearchBreedsItem>> SearchAsync(string? speciesName = null, bool? withAnimals = null,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Breeds
            .Where(breed => (speciesName == null || breed.Species.Name == speciesName) &&
                            (withAnimals == null || withAnimals.Value && breed.Animals.Count > 0))
            .Select(breed => new SearchBreedsItem
            {
                Id = breed.Id,
                Name = breed.Name,
                SpeciesName = breed.Species.Name,
                AnimalCount = breed.Animals.Count(animal => animal.Breed.Id == breed.Id)
            })
            .OrderBy(result => result.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Breed?> GetByIdAsync(int breedId, CancellationToken cancellationToken = default)
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
