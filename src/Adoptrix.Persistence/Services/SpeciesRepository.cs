using Adoptrix.Core;
using Adoptrix.Persistence.Responses;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Persistence.Services;

public interface ISpeciesRepository
{
    Task<IEnumerable<SearchSpeciesItem>> SearchAsync(bool? withAnimals = null, CancellationToken cancellationToken = default);
    Task<Species?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Species?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}

public class SpeciesRepository(AdoptrixDbContext dbContext): ISpeciesRepository
{
    public async Task<IEnumerable<SearchSpeciesItem>> SearchAsync(bool? withAnimals,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Species
            .Select(species => new SearchSpeciesItem
            {
                Id = species.Id,
                Name = species.Name,
                BreedCount = species.Breeds.Count,
                AnimalCount = species.Breeds.Count(breed => breed.Animals.Count > 0)
            })
            .Where(match => withAnimals == null || !withAnimals.Value || match.AnimalCount > 0)
            .OrderByDescending(match => match.AnimalCount)
            .ToListAsync(cancellationToken);
    }

    public async Task<Species?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Species.FirstOrDefaultAsync(species => species.Id == id, cancellationToken);
    }

    public async Task<Species?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await dbContext.Species.FirstOrDefaultAsync(species => species.Name == name, cancellationToken);
    }
}
