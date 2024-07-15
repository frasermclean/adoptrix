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
    IBlobContainerManager containerManager)
{
    [Function(nameof(CleanupDeletedAnimal))]
    public async Task ExecuteAsync([QueueTrigger(QueueNames.AnimalDeleted)] AnimalDeletedEvent data,
        CancellationToken cancellationToken = default)
    {
        var blobNames = await containerManager.GetBlobNamesAsync($"{data.AnimalId}/", cancellationToken);

        foreach (var blobName in blobNames)
        {
            await containerManager.DeleteBlobAsync(blobName, cancellationToken);
            logger.LogInformation("Deleted blob {BlobName}", blobName);
        }
    }
}
