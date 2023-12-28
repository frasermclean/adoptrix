using Adoptrix.Application.Services;
using Adoptrix.Domain.Events;
using Adoptrix.Infrastructure.Storage;
using Microsoft.Azure.Functions.Worker;

namespace Adoptrix.Functions.Functions;

public class AnimalDeleted(IAnimalImageManager animalImageManager)
{
    [Function(nameof(AnimalDeleted))]
    public async Task Run([QueueTrigger(QueueNames.AnimalDeleted)] AnimalDeletedEvent eventData)
    {
        await animalImageManager.DeleteAnimalImagesAsync(eventData.AnimalId);
    }
}