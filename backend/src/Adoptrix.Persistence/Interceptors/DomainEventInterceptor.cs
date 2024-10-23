using Adoptrix.Core;
using Adoptrix.Core.Events;
using Adoptrix.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Adoptrix.Persistence.Interceptors;

public class DomainEventInterceptor(IRequestContext requestContext, IEventPublisher eventPublisher)
    : SaveChangesInterceptor
{
    private List<(EntityState, object)>? changedEntities;

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        changedEntities = eventData.Context?.ChangeTracker.Entries()
            .Where(entry => entry is { State: EntityState.Added or EntityState.Modified or EntityState.Deleted })
            .Select(entry => (entry.State, entry.Entity))
            .ToList();

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = default)
    {
        var domainEvents = changedEntities?.Select(MapToDomainEvent).OfType<DomainEvent>() ?? [];

        var tasks = domainEvents.Select(domainEvent => eventPublisher.PublishAsync(domainEvent, cancellationToken));
        await Task.WhenAll(tasks);

        return result;
    }

    private DomainEvent? MapToDomainEvent((EntityState State, object Entity) tuple) => tuple switch
    {
        { State: EntityState.Added, Entity: AnimalImage image }
            => new AnimalImageAddedEvent(image.AnimalSlug, image.Id, image.OriginalBlobName),
        { State: EntityState.Deleted, Entity: Animal animal }
            => new AnimalDeletedEvent(animal.Slug, requestContext.UserId),
        _ => null
    };
}
