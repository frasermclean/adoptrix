namespace Adoptrix.Domain.Events;

public record AnimalImageAddedEvent(Guid AnimalId, Guid ImageId) : IDomainEvent;
