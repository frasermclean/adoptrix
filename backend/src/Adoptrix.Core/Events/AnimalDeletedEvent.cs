namespace Adoptrix.Core.Events;

public record AnimalDeletedEvent(string AnimalSlug) : IDomainEvent;
