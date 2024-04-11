using Adoptrix.Application.Notifications.Animals;
using Azure.Storage.Queues;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Storage.Handlers;

public sealed class AnimalDeletedNotificationHandler(
    ILogger<AnimalDeletedNotificationHandler> logger,
    [FromKeyedServices(QueueNames.AnimalDeleted)]
    QueueClient queueClient)
    : QueueSender(queueClient, logger), INotificationHandler<AnimalDeletedNotification>
{
    public Task Handle(AnimalDeletedNotification notification, CancellationToken cancellationToken)
    {
        return SendMessageAsync(notification, cancellationToken);
    }
}
