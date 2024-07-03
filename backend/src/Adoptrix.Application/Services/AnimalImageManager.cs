using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Domain.Models;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Services;

public sealed class AnimalImageManager(
    ILogger<AnimalImageManager> logger,
    [FromKeyedServices(BlobContainerNames.AnimalImages)]
    IBlobContainerManager containerManager)
    : IAnimalImageManager
{
    public Uri ContainerUri => containerManager.ContainerUri;

    public async Task<Result> UploadImageAsync(Guid animalId, Guid imageId, Stream imageStream, string contentType,
        AnimalImageCategory category = AnimalImageCategory.Original, CancellationToken cancellationToken = default)
    {
        var blobName = AnimalImage.GetBlobName(animalId, imageId, category);
        return await containerManager.UploadBlobAsync(blobName, imageStream, contentType, cancellationToken);
    }

    public async Task<Result<int>> DeleteAnimalImagesAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var blobNames = await containerManager.GetBlobNamesAsync("{animalId}/", cancellationToken);
        var count = 0;

        foreach (var blobName in blobNames)
        {
            var deleteResult = await containerManager.DeleteBlobAsync(blobName, cancellationToken);
            if (deleteResult.IsFailed) continue;

            logger.LogInformation("Deleted blob {BlobName}", blobName);
            count++;
        }


        return count;
    }

    public async Task<Result> DeleteImageAsync(Guid animalId, Guid imageId,
        AnimalImageCategory category = AnimalImageCategory.Original, CancellationToken cancellationToken = default)
    {
        var blobName = AnimalImage.GetBlobName(animalId, imageId, category);
        return await containerManager.DeleteBlobAsync(blobName, cancellationToken);
    }

    public Task<Stream> GetImageReadStreamAsync(Guid animalId, Guid imageId,
        AnimalImageCategory category = AnimalImageCategory.Original,
        CancellationToken cancellationToken = default)
    {
        var blobName = AnimalImage.GetBlobName(animalId, imageId, category);
        return containerManager.OpenReadStreamAsync(blobName, cancellationToken);
    }
}
