using MediatR;

namespace Adoptrix.Application.Notifications.Animals;

public record AnimalDeletedNotification(Guid AnimalId) : INotification;
