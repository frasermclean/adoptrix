namespace Adoptrix.Core.Events;

public record AnimalImageAddedEvent(int AnimalId, Guid ImageId, string BlobName) : IDomainEvent;
