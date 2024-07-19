namespace Adoptrix.Initializer.Services;

public class WorkerService(
    IServiceScopeFactory serviceScopeFactory,
    IHostApplicationLifetime hostApplicationLifetime,
    StorageInitializer storageInitializer)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        // wait a bit to let the database and storage services start
        await Task.Delay(5000, cancellationToken);

        await using var scope = serviceScopeFactory.CreateAsyncScope();

        // initialize database
        var databaseInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
        await databaseInitializer.EnsureCreatedAsync(cancellationToken);
        await databaseInitializer.SeedSpeciesAsync(SeedData.Species.Values, cancellationToken);
        await databaseInitializer.SeedBreedsAsync(SeedData.Breeds.Values, cancellationToken);
        await databaseInitializer.SeedAnimalsAsync(SeedData.Animals, cancellationToken);

        // initialize storage
        await storageInitializer.InitializeAsync(cancellationToken);

        hostApplicationLifetime.StopApplication();
    }
}
