using Adoptrix.Core;
using Adoptrix.Persistence.Services;

namespace Adoptrix.Initializer.Services;

public class DatabaseInitializer(ILogger<DatabaseInitializer> logger, AdoptrixDbContext dbContext)
{
    public async Task<bool> EnsureCreatedAsync(CancellationToken cancellationToken = default)
    {
        var wasCreated = await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        if (wasCreated)
        {
            logger.LogInformation("Database was created");
        }

        return wasCreated;
    }

    public async Task AddSpeciesAsync(IEnumerable<Species> species, CancellationToken cancellationToken = default)
    {
        dbContext.Species.AddRange(species);
        var count = await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Added {Count} species", count);
    }

    public async Task AddBreedsAsync(IEnumerable<Breed> breeds, CancellationToken cancellationToken = default)
    {
        dbContext.Breeds.AddRange(breeds);
        var count = await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Added {Count} breeds", count);
    }

    public async Task AddAnimalsAsync(IEnumerable<Animal> animals, CancellationToken cancellationToken = default)
    {
        dbContext.Animals.AddRange(animals);
        var count = await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Added {Count} animals", count);
    }
}
