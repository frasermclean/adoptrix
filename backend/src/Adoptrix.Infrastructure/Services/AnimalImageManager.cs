using Adoptrix.Application.Services;
using Adoptrix.Domain;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Infrastructure.Services;

public class AnimalImageManager(
    ILogger<AnimalImageManager> logger,
    [FromKeyedServices(AnimalImageManager.ContainerName)]
    BlobContainerClient containerClient)
    : IAnimalImageManager
{
    public const string ContainerName = "animal-images";

    public async Task UploadImageAsync(Guid animalId, ImageInformation information, Stream imageStream,
        CancellationToken cancellationToken)
    {
        var blobName = GetBlobName(animalId, information.Id);
        var blobClient = containerClient.GetBlobClient(blobName);

        var options = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = information.OriginalContentType }
        };

        await blobClient.UploadAsync(imageStream, options, cancellationToken);

        logger.LogInformation("Uploaded image {BlobName} with content type {ContentType} to blob storage",
            blobName, information.OriginalContentType);
    }

    public async Task<Result> DeleteImageAsync(Guid animalId, Guid imageId, CancellationToken cancellationToken)
    {
        var blobName = GetBlobName(animalId, imageId);
        var blobClient = containerClient.GetBlobClient(blobName);
        var response = await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots,
            cancellationToken: cancellationToken);

        return Result.OkIf(response.Value, "Specified blob was not found");
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