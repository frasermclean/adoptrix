using MediatR;

namespace Adoptrix.Application.Notifications.Animals;

public record AnimalImageAddedNotification(Guid AnimalId, Guid ImageId) : INotification;

