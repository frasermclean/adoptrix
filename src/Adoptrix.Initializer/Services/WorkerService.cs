namespace Adoptrix.Initializer.Services;

public class WorkerService(IServiceScopeFactory serviceScopeFactory, IHostApplicationLifetime hostApplicationLifetime)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        // wait for the database to be ready
        await Task.Delay(5000, cancellationToken);

        await using var scope = serviceScopeFactory.CreateAsyncScope();

        var databaseInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();

        await databaseInitializer.ApplyMigrationsAsync(cancellationToken);
        await databaseInitializer.SeedSpeciesAsync(SeedData.Species.Values, cancellationToken);
        await databaseInitializer.SeedBreedsAsync(SeedData.Breeds.Values, cancellationToken);
        await databaseInitializer.SeedAnimalsAsync(SeedData.Animals, cancellationToken);

        hostApplicationLifetime.StopApplication();
    }
}
