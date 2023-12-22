using FastEndpoints;

namespace Adoptrix.Application.Events;

public record AnimalImageAddedEvent(Guid AnimalId, Guid ImageId) : IEvent;
