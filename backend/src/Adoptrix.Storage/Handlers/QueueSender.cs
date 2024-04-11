using System.Text.Json;
using Azure.Storage.Queues;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Storage.Handlers;

public abstract class QueueSender(QueueClient queueClient, ILogger logger)
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    protected async Task SendMessageAsync<T>(T notification, CancellationToken cancellationToken = default)
        where T : INotification
    {
        var data = new BinaryData(notification, SerializerOptions);

        await queueClient.SendMessageAsync(data, cancellationToken: cancellationToken);

        logger.LogInformation("Published message {Notification} to queue {QueueName}",
            notification, queueClient.Name);
    }
}
