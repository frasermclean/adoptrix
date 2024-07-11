using Adoptrix.Core;
using Adoptrix.Core.Abstractions;
using Adoptrix.Core.Contracts.Requests.Animals;
using Adoptrix.Core.Contracts.Responses;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Persistence.Services;

public class AnimalsRepository(AdoptrixDbContext dbContext) : IAnimalsRepository
{
    private const int SearchLimit = 10;

    public async Task<IReadOnlyList<AnimalMatch>> SearchAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Animals
            .AsNoTracking()
            .Where(animal => (request.Name == null || animal.Name.Contains(request.Name)) &&
                             (request.BreedId == null || animal.Breed.Id == request.BreedId) &&
                             (request.SpeciesId == null || animal.Breed.Species.Id == request.SpeciesId) &&
                             (request.Sex == null || animal.Sex == request.Sex))
            .Take(request.Limit ?? SearchLimit)
            .Select(animal => new AnimalMatch
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
