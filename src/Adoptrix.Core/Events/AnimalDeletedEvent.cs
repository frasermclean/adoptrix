namespace Adoptrix.Core.Events;

public record AnimalDeletedEvent(int AnimalId) : IDomainEvent;
