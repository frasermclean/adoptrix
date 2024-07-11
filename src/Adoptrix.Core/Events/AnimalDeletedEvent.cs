namespace Adoptrix.Core.Events;

public record AnimalDeletedEvent(Guid AnimalId) : IDomainEvent;
