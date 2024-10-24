namespace Adoptrix.Core.Events;

public record AnimalDeletedEvent(string AnimalSlug, Guid UserId) : DomainEvent;
