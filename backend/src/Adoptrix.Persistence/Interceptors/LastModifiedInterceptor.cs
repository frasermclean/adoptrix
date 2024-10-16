using Adoptrix.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Adoptrix.Persistence.Interceptors;

public class LastModifiedInterceptor(IRequestContext requestContext) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        foreach (var entity in eventData.Context.ChangeTracker.Entries()
                     .Where(entry => entry is
                     {
                         Entity: ILastModifiedEntity,
                         State: EntityState.Added or EntityState.Modified
                     })
                     .Select(entry => entry.Entity)
                     .Cast<ILastModifiedEntity>())
        {
            entity.LastModifiedUtc = DateTime.UtcNow;
            entity.LastModifiedBy = requestContext.UserId;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
