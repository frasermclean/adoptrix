using System.Collections.Concurrent;
using FastEndpoints;

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
    void PushDomainEvent(IEvent domainEvent);

    /// <summary>
    /// Remove the next domain event from the queue.
    /// </summary>
    /// <exception cref="InvalidOperationException">There are no items left in the queue.</exception>
    IEvent? PopDomainEvent();
}

public class EventQueueService : IEventQueueService
{
    private readonly ConcurrentQueue<IEvent> domainEvents = new();

    public bool HasEvents => !domainEvents.IsEmpty;

    public void PushDomainEvent(IEvent domainEvent)
    {
        domainEvents.Enqueue(domainEvent);
    }

    public IEvent? PopDomainEvent()
    {
        domainEvents.TryDequeue(out var domainEvent);
        return domainEvent;
    }
}