using FluentResults;

namespace Adoptrix.Infrastructure.Services.Repositories;

public abstract class Repository(AdoptrixDbContext dbContext)
{
    protected AdoptrixDbContext DbContext { get; } = dbContext;

    protected async Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entriesChangedCount = await DbContext.SaveChangesAsync(cancellationToken);
        return Result.FailIf(entriesChangedCount == 0, "No database changes were made");
    }
}