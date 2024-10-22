using Adoptrix.Core;
using Adoptrix.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Adoptrix.Persistence.Interceptors;

public class AuditingInterceptor(IRequestContext requestContext)
    : SaveChangesInterceptor
{
    private List<AuditEntry> auditEntries = [];

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not AdoptrixDbContext dbContext)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        auditEntries = dbContext.ChangeTracker.Entries()
            .Where(entry => entry is
            {
                State: EntityState.Added or EntityState.Modified or EntityState.Deleted,
                Entity: not AuditEntry
            })
            .Select(entry => new AuditEntry
            {
                UserId = requestContext.UserId,
                OperationName = GetOperationName(entry)
            })
            .ToList();

        dbContext.AuditEntries.AddRange(auditEntries);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not AdoptrixDbContext dbContext || auditEntries.Count == 0)
        {
            return result;
        }

        var utcNow = DateTime.UtcNow;
        foreach (var auditEntry in auditEntries)
        {
            dbContext.Attach(auditEntry);

            auditEntry.WasSuccessful = true;
            auditEntry.EndTimeUtc = utcNow;
        }

        auditEntries.Clear();
        await dbContext.SaveChangesAsync(cancellationToken);

        return result;
    }

    public override async Task SaveChangesFailedAsync(DbContextErrorEventData eventData,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not AdoptrixDbContext dbContext || auditEntries.Count == 0)
        {
            return;
        }

        var utcNow = DateTime.UtcNow;
        foreach (var auditEntry in auditEntries)
        {
            dbContext.Attach(auditEntry);

            auditEntry.WasSuccessful = false;
            auditEntry.EndTimeUtc = utcNow;
            auditEntry.ErrorMessage = eventData.Exception.Message;
        }

        auditEntries.Clear();
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static string GetOperationName(EntityEntry entry) => entry switch
    {
        { State: EntityState.Added } => $"Add-{entry.Entity.GetType().Name}",
        { State: EntityState.Modified } => $"Update-{entry.Entity.GetType().Name}",
        { State: EntityState.Deleted } => $"Delete-{entry.Entity.GetType().Name}",
        _ => throw new InvalidOperationException("Unknown operation")
    };
}
