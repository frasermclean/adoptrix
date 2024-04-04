using Microsoft.Extensions.Logging;

namespace Adoptrix.Database.Services;

public interface IBatchManager
{
    void Start();
    bool IsStarted { get; }
}

public sealed class BatchManager(AdoptrixDbContext dbContext, ILogger<BatchManager> logger)
    : IBatchManager, IDisposable, IAsyncDisposable
{
    public bool IsStarted { get; private set; }

    public void Start()
    {
        IsStarted = true;
        logger.LogInformation("Started unit of work");
    }

    public void Dispose()
    {
        if (!IsStarted) return;

        var changeCount = dbContext.SaveChanges();
        logger.LogInformation("Saved {ChangeCount} changes", changeCount);
    }

    public async ValueTask DisposeAsync()
    {
        if (!IsStarted) return;

        var changeCount = await dbContext.SaveChangesAsync();
        logger.LogInformation("Saved {ChangeCount} changes", changeCount);
    }
}
