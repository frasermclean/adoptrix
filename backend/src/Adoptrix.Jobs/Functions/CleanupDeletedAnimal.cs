using Adoptrix.Core.Events;
using Adoptrix.Persistence;
using Adoptrix.Persistence.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Jobs.Functions;

public class CleanupDeletedAnimal(
    ILogger<CleanupDeletedAnimal> logger,
    [FromKeyedServices(BlobContainerNames.AnimalImages)]
    IBlobContainerManager animalImagesContainerManager)
{
    [Function(nameof(CleanupDeletedAnimal))]
    public async Task ExecuteAsync([QueueTrigger(QueueNames.AnimalDeleted)] AnimalDeletedEvent data,
        CancellationToken cancellationToken = default)
    {
        var blobPrefix = $"{data.AnimalSlug}/";
        var blobNames = await animalImagesContainerManager.GetBlobNamesAsync(blobPrefix, cancellationToken);

        foreach (var blobName in blobNames)
        {
            await animalImagesContainerManager.DeleteBlobAsync(blobName, cancellationToken);
            logger.LogInformation("Deleted blob {BlobName}", blobName);
        }
    }
}
