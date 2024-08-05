namespace Adoptrix.Core.Events;

public record AnimalImageAddedEvent(int AnimalId, int ImageId, string BlobName) : IDomainEvent;
