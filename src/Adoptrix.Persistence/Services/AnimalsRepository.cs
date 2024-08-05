using Adoptrix.Core;
using Adoptrix.Persistence.Responses;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Persistence.Services;

public interface IAnimalsRepository
{
    Task<IReadOnlyList<SearchAnimalsItem>> SearchAsync(string? name = null, int? breedId = null,
        string? speciesName = null, Sex? sex = null, int? limit = null, CancellationToken cancellationToken = default);

    Task<Animal?> GetByIdAsync(Guid animalId, CancellationToken cancellationToken = default);
    Task AddAsync(Animal animal, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task DeleteAsync(Animal animal, CancellationToken cancellationToken = default);
}

public class AnimalsRepository(AdoptrixDbContext dbContext) : IAnimalsRepository
{
    public async Task<IReadOnlyList<SearchAnimalsItem>> SearchAsync(string? name, int? breedId, string? speciesName,
        Sex? sex, int? limit, CancellationToken cancellationToken = default)
    {
        return await dbContext.Animals
            .AsNoTracking()
            .Where(animal => (name == null || animal.Name.Contains(name)) &&
                             (breedId == null || animal.Breed.Id == breedId) &&
                             (speciesName == null || animal.Breed.Species.Name == speciesName) &&
                             (sex == null || animal.Sex == sex))
            .Take(limit ?? 10)
            .Select(animal => new SearchAnimalsItem
            {
                Id = animal.Id,
                Name = animal.Name,
                SpeciesName = animal.Breed.Species.Name,
                BreedName = animal.Breed.Name,
                Sex = animal.Sex,
                DateOfBirth = animal.DateOfBirth,
                CreatedAt = animal.CreatedAt,
                Image = animal.Images.FirstOrDefault(),
            })
            .OrderBy(animal => animal.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Animal?> GetByIdAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Animals.Where(animal => animal.Id == animalId)
            .Include(animal => animal.Breed)
            .ThenInclude(breed => breed.Species)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        dbContext.Animals.Add(animal);
        await SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);

    public async Task DeleteAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        dbContext.Animals.Remove(animal);
        await SaveChangesAsync(cancellationToken);
    }
}
