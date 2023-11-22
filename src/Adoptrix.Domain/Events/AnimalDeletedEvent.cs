using FastEndpoints;

namespace Adoptrix.Domain.Events;

public record AnimalDeletedEvent(Animal Animal) : IEvent;