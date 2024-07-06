namespace Adoptrix.Core.Events;

public record AnimalImageAddedEvent(Guid AnimalId, Guid ImageId) : IDomainEvent;
