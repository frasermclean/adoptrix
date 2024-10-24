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
        foreach (var entry in eventData.Context?.ChangeTracker.Entries<IModifiable>()
                     .Where(entry => entry is { State: EntityState.Added or EntityState.Modified }) ?? [])
        {
            if (!requestContext.IsAuthenticated)
            {
                continue;
            }

            // update the last modified properties
            entry.Property(modifiable => modifiable.LastModifiedBy).CurrentValue = requestContext.UserId;
            entry.Property(modifiable => modifiable.LastModifiedUtc).CurrentValue = DateTime.UtcNow;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
