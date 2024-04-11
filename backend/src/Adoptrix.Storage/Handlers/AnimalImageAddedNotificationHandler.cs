using Adoptrix.Application.Notifications.Animals;
using Azure.Storage.Queues;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Storage.Handlers;

public sealed class AnimalImageAddedNotificationHandler(
    ILogger<AnimalImageAddedNotificationHandler> logger,
    [FromKeyedServices(QueueNames.AnimalImageAdded)]
    QueueClient queueClient)
    : QueueSender(queueClient, logger), INotificationHandler<AnimalImageAddedNotification>
{
    public Task Handle(AnimalImageAddedNotification notification, CancellationToken cancellationToken)
    {
        return SendMessageAsync(notification, cancellationToken);
    }
}
