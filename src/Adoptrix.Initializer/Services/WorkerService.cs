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
        await databaseInitializer.AddSpeciesAsync(SeedData.Species, cancellationToken);
        await databaseInitializer.AddBreedsAsync(SeedData.Breeds, cancellationToken);
        await databaseInitializer.AddAnimalsAsync(SeedData.Animals, cancellationToken);

        // initialize storage
        await storageInitializer.InitializeAsync(cancellationToken);

        hostApplicationLifetime.StopApplication();
    }
}
