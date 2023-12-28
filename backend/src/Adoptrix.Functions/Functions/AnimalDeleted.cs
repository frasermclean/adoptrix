using Adoptrix.Application.Services;
using Adoptrix.Domain.Events;
using Microsoft.Azure.Functions.Worker;

namespace Adoptrix.Functions.Functions;

public class AnimalDeleted(IAnimalImageManager animalImageManager)
{
    [Function(nameof(AnimalDeleted))]
    public async Task Run([QueueTrigger("animal-deleted")] AnimalDeletedEvent eventData)
    {
        await animalImageManager.DeleteAnimalImagesAsync(eventData.AnimalId);
    }
}