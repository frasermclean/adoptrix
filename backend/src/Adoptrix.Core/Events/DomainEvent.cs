namespace Adoptrix.Core.Events;

public abstract record DomainEvent
{
    public DateTime StartTimeUtc { get; } = DateTime.UtcNow;
}
