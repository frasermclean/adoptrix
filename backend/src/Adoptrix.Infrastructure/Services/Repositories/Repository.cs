using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Infrastructure.Services.Repositories;

public abstract class Repository<T>(AdoptrixDbContext dbContext, IBatchManager batchManager) where T : DbContext
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
