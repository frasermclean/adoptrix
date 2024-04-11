using MediatR;

namespace Adoptrix.Application.Notifications.Animals;

public record AnimalAddedNotification(Guid AnimalId) : INotification;
