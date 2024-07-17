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

    public async Task SeedDatabaseAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Seeding database");

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            await SeedSpeciesAsync(cancellationToken);
            await SeedBreedsAsync(cancellationToken);
            await SeedAnimalsAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        });
    }

    private async Task SeedSpeciesAsync(CancellationToken cancellationToken)
    {
        foreach (var species in SeedData.Species)
        {
            if (await dbContext.Species.FirstOrDefaultAsync(s => s.Id == species.Id, cancellationToken: cancellationToken) != null)
            {
                continue;
            }

            dbContext.Species.Add(species);
        }

        var saveCount = await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Seeded {SaveCount}/{SeedDataCount} species", saveCount, SeedData.Species.Length);
    }

    private async Task SeedBreedsAsync(CancellationToken cancellationToken)
    {
        foreach (var breed in SeedData.Breeds)
        {
            if (await dbContext.Breeds.FirstOrDefaultAsync(b => b.Id == breed.Id, cancellationToken: cancellationToken) != null)
            {
                continue;
            }

            dbContext.Breeds.Add(breed);
        }

        var saveCount = await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Seeded {SaveCount}/{SeedDataCount} breeds", saveCount, SeedData.Breeds.Length);
    }

    private async Task SeedAnimalsAsync(CancellationToken cancellationToken)
    {
        foreach (var animal in SeedData.Animals)
        {
            if (await dbContext.Animals.FirstOrDefaultAsync(a => a.Id == animal.Id, cancellationToken: cancellationToken) != null)
            {
                continue;
            }

            dbContext.Animals.Add(animal);
        }

        var saveCount = await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Seeded {SaveCount}/{SeedDataCount} animals", saveCount, SeedData.Animals.Length);
    }
}
