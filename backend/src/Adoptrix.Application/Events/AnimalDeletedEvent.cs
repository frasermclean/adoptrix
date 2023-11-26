using Adoptrix.Domain;
using FastEndpoints;

namespace Adoptrix.Application.Events;

public record AnimalDeletedEvent(Animal Animal) : IEvent;