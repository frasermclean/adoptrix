using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Notifications.Animals;
using Adoptrix.Storage;
using MediatR;
using Microsoft.Azure.Functions.Worker;

namespace Adoptrix.Jobs.Functions;

public class AnimalFunctions(ISender sender)
{
    [Function(nameof(ProcessAnimalImage))]
    public async Task ProcessAnimalImage([QueueTrigger(QueueNames.AnimalImageAdded)] AnimalImageAddedNotification notification)
    {
        var result = await sender.Send(new ProcessAnimalImageRequest(notification.AnimalId, notification.ImageId));

        if (result.IsFailed)
        {
            throw new Exception(
                $"Failed to process image with ID: {notification.ImageId} for animal with ID: {notification.AnimalId}");
        }
    }

    [Function(nameof(CleanupDeletedAnimal))]
    public async Task CleanupDeletedAnimal([QueueTrigger(QueueNames.AnimalDeleted)] AnimalDeletedNotification notification)
    {
        var result = await sender.Send(new CleanupAnimalImagesRequest(notification.AnimalId));

        if (result.IsFailed)
        {
            throw new Exception(
                $"Failed to cleanup images for animal with ID: {notification.AnimalId}");
        }
    }
}
