using Adoptrix.Application.Services;
using Adoptrix.Domain.Events;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Functions.Functions;

public class AnimalDeleted(ILogger<AnimalDeleted> logger, IAnimalImageManager animalImageManager)
{
    [Function(nameof(AnimalDeleted))]
    public async Task Run([QueueTrigger("animal-deleted")] AnimalDeletedEvent eventData)
    {
        await animalImageManager.DeleteAnimalImagesAsync(eventData.AnimalId);
    }
}