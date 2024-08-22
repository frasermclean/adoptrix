namespace Adoptrix.Core.Events;

public record AnimalImageAddedEvent(string AnimalSlug, int ImageId, string BlobName) : IDomainEvent;
