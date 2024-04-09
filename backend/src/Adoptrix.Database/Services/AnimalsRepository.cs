using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Database.Services;

public class AnimalsRepository(AdoptrixDbContext dbContext, IBatchManager batchManager)
    : Repository(dbContext, batchManager), IAnimalsRepository
{
    public async Task<IEnumerable<SearchAnimalsResult>> SearchAsync(string? animalName = null,
        Guid? breedId = null, CancellationToken cancellationToken = default)
    {
        return await DbContext.Animals
            .AsNoTracking()
            .Where(animal => (animalName == null || animal.Name.Contains(animalName)) &&
                             (breedId == null || animal.Breed.Id == breedId))
            .Select(animal => new SearchAnimalsResult
            {
                Id = animal.Id,
                Name = animal.Name,
                SpeciesName = animal.Breed.Species.Name,
                BreedName = animal.Breed.Name,
                Sex = animal.Sex,
                DateOfBirth = animal.DateOfBirth,
                CreatedAt = animal.CreatedAt,
                Image = animal.Images.Select(image => new AnimalImageResponse
                    {
                        Id = image.Id, Description = image.Description, IsProcessed = image.IsProcessed
                    })
                    .FirstOrDefault(),
            })
            .OrderBy(animal => animal.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Animal?> GetByIdAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Animals.Where(animal => animal.Id == animalId)
            .Include(animal => animal.Breed)
            .ThenInclude(breed => breed.Species)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        DbContext.Animals.Add(animal);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        DbContext.Animals.Update(animal);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        DbContext.Animals.Remove(animal);
        await SaveChangesAsync(cancellationToken);
    }
}
