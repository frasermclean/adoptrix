namespace Adoptrix.Core.Events;

public record AnimalImageAddedEvent(string AnimalSlug, Guid ImageId, string BlobName) : IDomainEvent;
