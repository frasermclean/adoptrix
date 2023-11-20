using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Infrastructure.Services.Repositories;

public class AnimalsRepository(AdoptrixDbContext dbContext)
    : IAnimalsRepository
{
    public async Task<IEnumerable<Animal>> SearchAsync(string? name = null, Species? species = null,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Animals
            .ToListAsync(cancellationToken);
    }

    public async Task<Animal> AddAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        var entry = await dbContext.Animals.AddAsync(animal, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return entry.Entity;
    }
}