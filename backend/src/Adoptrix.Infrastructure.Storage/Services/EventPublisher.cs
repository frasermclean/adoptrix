using System.Text.Json;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Events;
using Azure.Storage.Queues;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Infrastructure.Storage.Services;

public class EventPublisher(
    ILogger<EventPublisher> logger,
    [FromKeyedServices(QueueNames.AnimalDeleted)]
    QueueClient animalDeletedQueueClient,
    [FromKeyedServices(QueueNames.AnimalImageAdded)]
    QueueClient animalImageAddedQueueClient
) : IEventPublisher
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task<Result> PublishDomainEventAsync<T>(T domainEvent, CancellationToken cancellationToken = default)
        where T : IDomainEvent
    {
        var data = new BinaryData(domainEvent, SerializerOptions);

        var queueClient = GetQueueClient(domainEvent);

        logger.LogInformation("Publishing domain event {DomainEvent} to queue {QueueName}",
            domainEvent.GetType().Name, queueClient.Name);

        var response = await queueClient.SendMessageAsync(data, cancellationToken: cancellationToken);
        var httpResponse = response.GetRawResponse();

        return Result.FailIf(httpResponse.IsError, "Failed to publish domain event to queue");
    }

    private QueueClient GetQueueClient<T>(T domainEvent) where T : IDomainEvent =>
        domainEvent switch
        {
            AnimalDeletedEvent => animalDeletedQueueClient,
            AnimalImageAddedEvent => animalImageAddedQueueClient,
            _ => throw new ArgumentOutOfRangeException(nameof(domainEvent))
        };
}