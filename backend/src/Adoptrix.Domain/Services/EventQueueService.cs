using System.Collections.Concurrent;
using Adoptrix.Domain.Events;

namespace Adoptrix.Domain.Services;

public interface IEventQueueService
{
    /// <summary>
    /// True if there are any domain events in the queue.
    /// </summary>
    bool HasEvents { get; }

    /// <summary>
    /// Add a domain event to the queue.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    void PushDomainEvent(IDomainEvent domainEvent);

    /// <summary>
    /// Remove the next domain event from the queue.
    /// </summary>
    /// <exception cref="InvalidOperationException">There are no items left in the queue.</exception>
    IDomainEvent? PopDomainEvent();
}

public class EventQueueService : IEventQueueService
{
    private readonly ConcurrentQueue<IDomainEvent> domainEvents = new();

    public bool HasEvents => !domainEvents.IsEmpty;

    public void PushDomainEvent(IDomainEvent domainEvent)
    {
        domainEvents.Enqueue(domainEvent);
    }

    public IDomainEvent? PopDomainEvent()
    {
        domainEvents.TryDequeue(out var domainEvent);
        return domainEvent;
    }
}