using Adoptrix.Core;
using Adoptrix.Persistence.Services;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Initializer.Services;

public class DatabaseInitializer(ILogger<DatabaseInitializer> logger, AdoptrixDbContext dbContext)
{
    public async Task ApplyMigrationsAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Applying migrations");

        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }

    public async Task SeedSpeciesAsync(IReadOnlyCollection<Species> speciesCollection,
        CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            foreach (var species in speciesCollection)
            {
                if (await dbContext.Species.FirstOrDefaultAsync(s => s.Id == species.Id,
                        cancellationToken: cancellationToken) != null)
                {
                    continue;
                }

                dbContext.Species.Add(species);
            }

            var saveCount = await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            logger.LogInformation("Seeded {SaveCount}/{SeedDataCount} species", saveCount, speciesCollection.Count);
        });
    }

    public async Task SeedBreedsAsync(IReadOnlyCollection<Breed> breeds, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            foreach (var breed in breeds)
            {
                if (await dbContext.Breeds.FirstOrDefaultAsync(b => b.Id == breed.Id,
                        cancellationToken: cancellationToken) != null)
                {
                    continue;
                }

                dbContext.Breeds.Add(breed);
            }

            var saveCount = await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            logger.LogInformation("Seeded {SaveCount}/{SeedDataCount} breeds", saveCount, breeds.Count);
        });
    }

    public async Task SeedAnimalsAsync(IReadOnlyCollection<Animal> animals, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            foreach (var animal in animals)
            {
                if (await dbContext.Animals.FirstOrDefaultAsync(a => a.Id == animal.Id,
                        cancellationToken: cancellationToken) != null)
                {
                    continue;
                }

                dbContext.Animals.Add(animal);
            }

            var saveCount = await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            logger.LogInformation("Seeded {SaveCount}/{SeedDataCount} animals", saveCount, animals.Count);
        });
    }
}
