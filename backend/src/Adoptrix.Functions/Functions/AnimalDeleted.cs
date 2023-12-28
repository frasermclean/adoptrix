using Adoptrix.Application.Services;
using Adoptrix.Domain.Events;
using Adoptrix.Infrastructure.Storage;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Functions.Functions;

public class AnimalDeleted(ILogger<AnimalDeleted> logger, IAnimalImageManager animalImageManager)
{
    [Function(nameof(AnimalDeleted))]
    public async Task Run([QueueTrigger(QueueNames.AnimalDeleted)] AnimalDeletedEvent eventData)
    {
        var result = await animalImageManager.DeleteAnimalImagesAsync(eventData.AnimalId);
        if (result.IsFailed)
        {
            logger.LogError("Failed to delete animal images for animal {AnimalId}: {Error}",
                eventData.AnimalId, result.Errors.First().Message);
            throw new Exception("Failed to delete animal images");
        }

        logger.LogInformation("Deleted {Count} images for animal {AnimalId}", result.Value, eventData.AnimalId);
    }
}