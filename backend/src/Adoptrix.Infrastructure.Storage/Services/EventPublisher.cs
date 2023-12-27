using System.Text.Json;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Events;
using Azure.Storage.Queues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Infrastructure.Storage.Services;

public class EventPublisher(
    ILogger<EventPublisher> logger,
    [FromKeyedServices(QueueKeys.AnimalDeleted)]
    QueueClient animalDeletedQueueClient,
    [FromKeyedServices(QueueKeys.AnimalImageAdded)]
    QueueClient animalImageAddedQueueClient
) : IEventPublisher
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task PublishDomainEventAsync<T>(T domainEvent, CancellationToken cancellationToken = default)
        where T : IDomainEvent
    {
        var data = new BinaryData(domainEvent, SerializerOptions);

        var queueClient = domainEvent switch
        {
            AnimalDeletedEvent => animalDeletedQueueClient,
            AnimalImageAddedEvent => animalImageAddedQueueClient,
            _ => throw new ArgumentOutOfRangeException(nameof(domainEvent))
        };

        logger.LogInformation("Publishing domain event {DomainEvent} to queue {QueueName}",
            domainEvent.GetType().Name, queueClient.Name);

        await queueClient.SendMessageAsync(data, cancellationToken: cancellationToken);
    }
}