using Adoptrix.Application.Events;
using Adoptrix.Application.Services;
using Azure.Storage.Queues;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Infrastructure.Services;

public class EventPublisher(
    [FromKeyedServices(QueueKeys.AnimalDeleted)]
    QueueClient animalDeletedQueueClient,
    [FromKeyedServices(QueueKeys.AnimalImageAdded)]
    QueueClient animalImageAddedQueueClient
)
    : IEventPublisher
{
    public async Task PublishAnimalDeletedEventAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        await animalDeletedQueueClient.SendMessageAsync(animalId.ToString(), cancellationToken);
    }

    public async Task PublishAnimalImageAddedEventAsync(Guid animalId, Guid imageId,
        CancellationToken cancellationToken = default)
    {
        var data = new BinaryData(new AnimalImageAddedEvent(animalId, imageId));
        await animalImageAddedQueueClient.SendMessageAsync(data, cancellationToken: cancellationToken);
    }
}