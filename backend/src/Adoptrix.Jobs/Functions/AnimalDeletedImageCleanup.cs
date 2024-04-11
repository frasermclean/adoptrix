using Adoptrix.Application.Notifications.Animals;
using Adoptrix.Application.Services;
using Adoptrix.Storage;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Jobs.Functions;

public class AnimalDeletedImageCleanup(ILogger<AnimalDeletedImageCleanup> logger, IAnimalImageManager animalImageManager)
{
    [Function(nameof(AnimalDeletedImageCleanup))]
    public async Task Run([QueueTrigger(QueueNames.AnimalDeleted)] AnimalDeletedNotification notification)
    {
        var result = await animalImageManager.DeleteAnimalImagesAsync(notification.AnimalId);
        if (result.IsFailed)
        {
            throw new Exception($"Failed to delete animal images for animal with ID: {notification.AnimalId}");
        }

        logger.LogInformation("Deleted {Count} images for animal {AnimalId}", result.Value, notification.AnimalId);
    }
}
