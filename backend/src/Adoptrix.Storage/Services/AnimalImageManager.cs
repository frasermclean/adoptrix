using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Support;
using Azure.Storage.Blobs;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Storage.Services;

public sealed class AnimalImageManager(
    ILogger<AnimalImageManager> logger,
    [FromKeyedServices(BlobContainerNames.AnimalImages)]
    BlobContainerClient containerClient)
    : BlobContainerManager(containerClient), IAnimalImageManager
{
    public async Task<Result> UploadImageAsync(Guid animalId, Guid imageId, Stream imageStream, string contentType,
        ImageCategory category = ImageCategory.Original, CancellationToken cancellationToken = default)
    {
        var blobName = GetBlobName(animalId, imageId, category);
        return await UploadBlobAsync(blobName, imageStream, contentType, cancellationToken);
    }

    public async Task<Result<int>> DeleteAnimalImagesAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var pages = ContainerClient.GetBlobsAsync(prefix: $"{animalId}/", cancellationToken: cancellationToken)
            .AsPages();

        var count = 0;
        await foreach (var page in pages)
        {
            foreach (var item in page.Values)
            {
                var deleteResult = await DeleteBlobAsync(item.Name, cancellationToken);
                if (deleteResult.IsFailed) continue;

                logger.LogInformation("Deleted blob {BlobName}", item.Name);
                count++;
            }
        }

        return count;
    }

    public async Task<Result> DeleteImageAsync(Guid animalId, Guid imageId,
        ImageCategory category = ImageCategory.Original, CancellationToken cancellationToken = default)
    {
        var blobName = GetBlobName(animalId, imageId, category);
        return await DeleteBlobAsync(blobName, cancellationToken);
    }

    public Task<Stream> GetImageReadStreamAsync(Guid animalId, Guid imageId, ImageCategory category = ImageCategory.Original,
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
