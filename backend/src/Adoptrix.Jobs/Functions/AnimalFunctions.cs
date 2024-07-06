using Adoptrix.Application.Services;
using Adoptrix.Core.Events;
using Adoptrix.Storage;
using Microsoft.Azure.Functions.Worker;

namespace Adoptrix.Jobs.Functions;

public class AnimalFunctions(IAnimalImageManager animalImageManager)
{
    [Function(nameof(ProcessAnimalImage))]
    public async Task ProcessAnimalImage([QueueTrigger(QueueNames.AnimalImageAdded)] AnimalImageAddedEvent eventData)
    {
        var result = await animalImageManager.ProcessOriginalAsync(eventData.AnimalId, eventData.ImageId);

        if (result.IsFailed)
        {
            throw new Exception(
                $"Failed to process image with ID: {eventData.ImageId} for animal with ID: {eventData.AnimalId}");
        }
    }

    [Function(nameof(CleanupDeletedAnimal))]
    public async Task CleanupDeletedAnimal([QueueTrigger(QueueNames.AnimalDeleted)] AnimalDeletedEvent eventData)
    {
        var result = await animalImageManager.DeleteAnimalImagesAsync(eventData.AnimalId);

        if (result.IsFailed)
        {
            throw new Exception($"Failed to cleanup images for animal with ID: {eventData.AnimalId}");
        }
    }
}
