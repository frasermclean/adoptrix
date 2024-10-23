namespace Adoptrix.Core.Events;

public interface IDomainEvent
{
    DateTime StartTimeUtc { get; }
}
