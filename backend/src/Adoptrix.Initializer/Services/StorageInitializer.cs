using Adoptrix.Persistence;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Queues;

namespace Adoptrix.Initializer.Services;

public class StorageInitializer(
    ILogger<StorageInitializer> logger,
    BlobServiceClient blobServiceClient,
    QueueServiceClient queueServiceClient)
{
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await InitializeBlobContainersAsync(cancellationToken);
        await InitializeQueuesAsync(cancellationToken);
    }

    private async Task InitializeBlobContainersAsync(CancellationToken cancellationToken)
    {
        // create animal images container (with public access)
        var animalImagesContainerClient = blobServiceClient.GetBlobContainerClient(BlobContainerNames.AnimalImages);
        await animalImagesContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob,
            cancellationToken: cancellationToken);

        // create original images container
        var originalImagesContainerClient = blobServiceClient.GetBlobContainerClient(BlobContainerNames.OriginalImages);
        await originalImagesContainerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        logger.LogInformation("Blob containers initialized");
    }

    private async Task InitializeQueuesAsync(CancellationToken cancellationToken)
    {
        var queuesToCreates = new[]
        {
            QueueNames.AnimalDeleted, QueueNames.AnimalImageAdded
        };

        foreach (var queueName in queuesToCreates)
        {
            var queueClient = queueServiceClient.GetQueueClient(queueName);
            await queueClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

            logger.LogInformation("Queue {QueueName} initialized", queueName);
        }
    }
}
