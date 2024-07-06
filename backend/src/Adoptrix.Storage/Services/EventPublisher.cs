using System.Text.Json;
using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Core.Events;
using Azure.Storage.Queues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Storage.Services;

public class EventPublisher(ILogger<EventPublisher> logger, IServiceProvider serviceProvider) : IEventPublisher
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default)
        where T : IDomainEvent
    {
        // serialize domain event
        var data = new BinaryData(domainEvent, SerializerOptions);

        // publish domain event to queue
        var queueClient = GetQueueClient(domainEvent);
        var response = await queueClient.SendMessageAsync(data, cancellationToken: cancellationToken);

        logger.LogInformation("Published {DomainEvent} to queue: {QueueName} - Message ID: {MessageId}",
            domainEvent, queueClient.Name, response.Value.MessageId);
    }

    private QueueClient GetQueueClient<T>(T domainEvent) where T : IDomainEvent =>
        domainEvent switch
        {
            AnimalDeletedEvent => serviceProvider.GetRequiredKeyedService<QueueClient>(QueueNames.AnimalDeleted),
            AnimalImageAddedEvent => serviceProvider.GetRequiredKeyedService<QueueClient>(QueueNames.AnimalImageAdded),
            _ => throw new ArgumentOutOfRangeException(nameof(domainEvent))
        };
}
