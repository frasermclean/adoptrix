using FluentResults;

namespace Adoptrix.Database.Services;

public abstract class Repository(AdoptrixDbContext dbContext, IBatchManager batchManager)
{
    protected AdoptrixDbContext DbContext { get; } = dbContext;

    protected async Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // batch manager is responsible for saving changes
        if (batchManager.IsStarted)
            return Result.Ok();

        var entriesChangedCount = await DbContext.SaveChangesAsync(cancellationToken);
        return Result.FailIf(entriesChangedCount == 0, "No database changes were made");
    }
}
