using Adoptrix.Application.Services;
using Adoptrix.Domain;
using Adoptrix.Infrastructure.Storage.DependencyInjection;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Infrastructure.Storage.Services;

public sealed class AnimalImageManager(
    ILogger<AnimalImageManager> logger,
    [FromKeyedServices(BlobContainerKeys.AnimalImages)]
    BlobContainerClient containerClient)
    : BlobContainerManager(containerClient), IAnimalImageManager
{
    public async Task<Result> UploadImageAsync(Guid animalId, Guid imageId, Stream imageStream, string contentType,
        ImageCategory category, CancellationToken cancellationToken = default)
    {
        var blobName = GetBlobName(animalId, imageId, category);
        return await UploadBlobAsync(blobName, imageStream, contentType, cancellationToken);
    }

    public async Task DeleteAnimalImagesAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var pages = ContainerClient.GetBlobsAsync(prefix: $"{animalId}/", cancellationToken: cancellationToken)
            .AsPages();

        await foreach (var page in pages)
        {
            foreach (var item in page.Values)
            {
                await ContainerClient.DeleteBlobAsync(item.Name, DeleteSnapshotsOption.IncludeSnapshots,
                    cancellationToken: cancellationToken);
                logger.LogInformation("Deleted blob {BlobName}", item.Name);
            }
        }
    }

    public async Task<Result> DeleteImageAsync(Guid animalId, Guid imageId, ImageCategory category,
        CancellationToken cancellationToken = default)
    {
        var blobName = GetBlobName(animalId, imageId, category);
        return await DeleteBlobAsync(blobName, cancellationToken);
    }

    public Task<Stream> GetImageReadStreamAsync(Guid animalId, Guid imageId, ImageCategory category,
        CancellationToken cancellationToken = default)
    {
        var blobName = GetBlobName(animalId, imageId, category);
        return OpenReadStreamAsync(blobName, cancellationToken);
    }

    private static string GetBlobName(Guid animalId, Guid imageId, ImageCategory category = ImageCategory.Original)
    {
        var suffix = category switch
        {
            ImageCategory.FullSize => "full",
            ImageCategory.Thumbnail => "thumb",
            ImageCategory.Preview => "preview",
            _ => "original"
        };

        return $"{animalId}/{imageId}/{suffix}";
    }
}