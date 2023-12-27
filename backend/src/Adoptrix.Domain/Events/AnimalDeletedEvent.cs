namespace Adoptrix.Domain.Events;

public record AnimalDeletedEvent(Guid AnimalId) : IDomainEvent;
