using Adoptrix.Persistence.Services;

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
        var dbContext = scope.ServiceProvider.GetRequiredService<AdoptrixDbContext>();
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);

        // initialize storage
        await storageInitializer.InitializeAsync(cancellationToken);

        hostApplicationLifetime.StopApplication();
    }
}
