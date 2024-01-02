using Adoptrix.Application.Services;
using Adoptrix.Domain.Events;
using Adoptrix.Infrastructure.Storage;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Jobs.Functions;

public class AnimalDeletedImageCleanup(ILogger<AnimalDeletedImageCleanup> logger, IAnimalImageManager animalImageManager)
{
    [Function(nameof(AnimalDeletedImageCleanup))]
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