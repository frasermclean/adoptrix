using Adoptrix.Core;
using Adoptrix.Logic.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Persistence.Services;

public class AnimalsRepository(AdoptrixDbContext dbContext) : IAnimalsRepository
{
    public async Task<Animal?> GetAsync(Guid animalId, CancellationToken cancellationToken)
    {
        var animal = await dbContext.Animals.Where(animal => animal.Id == animalId)
            .Include(animal => animal.Breed)
            .ThenInclude(breed => breed.Species)
            .FirstOrDefaultAsync(cancellationToken);

        return animal;
    }

    public async Task AddAsync(Animal animal, CancellationToken cancellationToken)
    {
        dbContext.Animals.Add(animal);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
        dbContext.SaveChangesAsync(cancellationToken);
}
