using Adoptrix.Application.Services;
using Adoptrix.Domain;
using Azure.Storage.Blobs;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Infrastructure.Services;

public sealed class AnimalImageManager(
    [FromKeyedServices(AnimalImageManager.ContainerName)]
    BlobContainerClient containerClient)
    : BlobContainerManager(containerClient), IAnimalImageManager
{
    public const string ContainerName = "animal-images";

    public async Task UploadImageAsync(Guid animalId, Guid imageId, Stream imageStream, string contentType,
        ImageCategory category, CancellationToken cancellationToken)
    {
        var blobName = GetBlobName(animalId, imageId, category);
        await UploadBlobAsync(blobName, imageStream, contentType, cancellationToken);
    }

    public async Task<Result> DeleteImageAsync(Guid animalId, Guid imageId, ImageCategory category,
        CancellationToken cancellationToken)
    {
        var blobName = GetBlobName(animalId, imageId, category);
        return await DeleteBlobAsync(blobName, cancellationToken);
    }

    public Task<Stream> GetImageReadStreamAsync(Guid animalId, Guid imageId, ImageCategory category,
        CancellationToken cancellationToken)
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