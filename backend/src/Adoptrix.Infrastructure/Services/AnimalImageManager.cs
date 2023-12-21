using Adoptrix.Application.Services;
using Adoptrix.Domain;
using Azure.Storage.Blobs;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Infrastructure.Services;

public sealed class AnimalImageManager(
    ILogger<AnimalImageManager> logger,
    [FromKeyedServices(AnimalImageManager.ContainerName)]
    BlobContainerClient containerClient)
    : BlobContainerManager(containerClient), IAnimalImageManager
{
    public const string ContainerName = "animal-images";

    public async Task UploadImageAsync(Guid animalId, ImageInformation information, Stream imageStream,
        CancellationToken cancellationToken)
    {
        var blobName = GetBlobName(animalId, information.Id);
        await UploadBlobAsync(blobName, imageStream, information.OriginalContentType, cancellationToken);

        logger.LogInformation("Uploaded image {BlobName} with content type {ContentType} to blob storage",
            blobName, information.OriginalContentType);
    }

    public async Task<Result> DeleteImageAsync(Guid animalId, Guid imageId, CancellationToken cancellationToken)
    {
        var blobName = GetBlobName(animalId, imageId);
        var result = await DeleteBlobAsync(blobName, cancellationToken);
        return result;
    }

    public Task<Stream> GetOriginalImageAsync(Guid animalId, Guid imageId, CancellationToken cancellationToken = default)
    {
        var blobName = GetBlobName(animalId, imageId);
        return GetBlobStreamAsync(blobName, cancellationToken);
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