using Adoptrix.Domain.Commands.Animals;
using Adoptrix.Domain.Events;
using Adoptrix.Storage;
using MediatR;
using Microsoft.Azure.Functions.Worker;

namespace Adoptrix.Jobs.Functions;

public class AnimalFunctions(ISender sender)
{
    [Function(nameof(ProcessAnimalImage))]
    public async Task ProcessAnimalImage([QueueTrigger(QueueNames.AnimalImageAdded)] AnimalImageAddedEvent eventData)
    {
        var result = await sender.Send(new ProcessAnimalImageCommand(eventData.AnimalId, eventData.ImageId));

        if (result.IsFailed)
        {
            throw new Exception(
                $"Failed to process image with ID: {eventData.ImageId} for animal with ID: {eventData.AnimalId}");
        }
    }

    [Function(nameof(CleanupDeletedAnimal))]
    public async Task CleanupDeletedAnimal([QueueTrigger(QueueNames.AnimalDeleted)] AnimalDeletedEvent eventData)
    {
        var result = await sender.Send(new CleanupAnimalImagesCommand(eventData.AnimalId));

        if (result.IsFailed)
        {
            throw new Exception(
                $"Failed to cleanup images for animal with ID: {eventData.AnimalId}");
        }
    }
}
