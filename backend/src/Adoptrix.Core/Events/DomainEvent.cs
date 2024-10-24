namespace Adoptrix.Core.Events;

public abstract record DomainEvent : IDomainEvent
{
    public DateTime StartTimeUtc { get; } = DateTime.UtcNow;
}
