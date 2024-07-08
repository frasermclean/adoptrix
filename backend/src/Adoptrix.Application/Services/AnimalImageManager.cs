using Adoptrix.Application.Mapping;
using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Core;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Core.Errors;
using Adoptrix.Core.Events;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Services;

public interface IAnimalImageManager
{
    Uri ContainerUri { get; }

    Task<Result<AnimalImage>> UploadOriginalAsync(Guid animalId, Guid userId, string fileName, string description,
        string contentType, Stream stream, CancellationToken cancellationToken = default);

    Task<Result<AnimalResponse>> AddImagesToAnimalAsync(Animal animal, IReadOnlyCollection<AnimalImage> images,
        CancellationToken cancellationToken = default);

    Task<Result> ProcessOriginalAsync(Guid animalId, Guid imageId, CancellationToken cancellationToken = default);

    Task<Result<int>> DeleteAnimalImagesAsync(Guid animalId, CancellationToken cancellationToken = default);
}

public sealed class AnimalImageManager(
    ILogger<AnimalImageManager> logger,
    IAnimalsRepository animalsRepository,
    [FromKeyedServices(BlobContainerNames.AnimalImages)]
    IBlobContainerManager containerManager,
    IEventPublisher eventPublisher,
    IImageProcessor imageProcessor)
    : IAnimalImageManager
{
    public Uri ContainerUri => containerManager.ContainerUri;

    public async Task<Result<AnimalImage>> UploadOriginalAsync(Guid animalId, Guid userId, string fileName,
        string description, string contentType, Stream stream, CancellationToken cancellationToken = default)
    {
        var image = new AnimalImage
        {
            Description = description,
            OriginalFileName = fileName,
            OriginalContentType = contentType,
            UploadedBy = userId
        };

        var uploadResult = await UploadImageAsync(animalId, image.Id, stream, contentType, AnimalImageCategory.Original,
            cancellationToken);

        return uploadResult.IsSuccess
            ? image
            : uploadResult;
    }

    public async Task<Result<AnimalResponse>> AddImagesToAnimalAsync(Animal animal,
        IReadOnlyCollection<AnimalImage> images, CancellationToken cancellationToken = default)
    {
        animal.Images.AddRange(images);
        await animalsRepository.SaveChangesAsync(cancellationToken);

        foreach (var image in images)
        {
            await eventPublisher.PublishAsync(new AnimalImageAddedEvent(animal.Id, image.Id), cancellationToken);
        }

        return animal.ToResponse();
    }

    public async Task<Result> ProcessOriginalAsync(Guid animalId, Guid imageId,
        CancellationToken cancellationToken = default)
    {
        // ensure animal exists in database
        var animal = await animalsRepository.GetByIdAsync(animalId, cancellationToken);
        if (animal is null)
        {
            logger.LogError("Could not find animal with ID {AnimalId}, will not process original", animalId);
            return new AnimalNotFoundError(animalId);
        }

        // process original image
        await using var originalReadStream =
            await GetImageReadStreamAsync(animalId, imageId, AnimalImageCategory.Original, cancellationToken);
        await using var bundle = await imageProcessor.ProcessOriginalAsync(originalReadStream, cancellationToken);

        // upload processed images
        await UploadProcessedBundleAsync(animalId, imageId, bundle, cancellationToken);

        // update entity in database
        var image = animal.Images.FirstOrDefault(image => image.Id == imageId);
        if (image is null)
        {
            return new AnimalImageNotFoundError(imageId, animal.Id);
        }

        image.IsProcessed = true;
        await animalsRepository.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }

    public async Task<Result<int>> DeleteAnimalImagesAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var blobNames = await containerManager.GetBlobNamesAsync($"{animalId}/", cancellationToken);
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

    private async Task UploadProcessedBundleAsync(Guid animalId, Guid imageId, ImageStreamBundle bundle, CancellationToken cancellationToken)
    {
        await Task.WhenAll(
            UploadImageAsync(animalId, imageId, bundle.ThumbnailWriteStream,
                ImageProcessor.OutputContentType, AnimalImageCategory.Thumbnail, cancellationToken),
            UploadImageAsync(animalId, imageId, bundle.PreviewWriteStream,
                ImageProcessor.OutputContentType, AnimalImageCategory.Preview, cancellationToken),
            UploadImageAsync(animalId, imageId, bundle.FullSizeWriteStream,
                ImageProcessor.OutputContentType, AnimalImageCategory.FullSize, cancellationToken)
        );

        logger.LogInformation("Uploaded processed images for animal with ID: {AnimalId}", animalId);
    }

    private async Task<Result> UploadImageAsync(Guid animalId, Guid imageId, Stream imageStream, string contentType,
        AnimalImageCategory category = AnimalImageCategory.Original, CancellationToken cancellationToken = default)
    {
        var blobName = AnimalImage.GetBlobName(animalId, imageId, category);
        return await containerManager.UploadBlobAsync(blobName, imageStream, contentType, cancellationToken);
    }

    private Task<Stream> GetImageReadStreamAsync(Guid animalId, Guid imageId,
        AnimalImageCategory category = AnimalImageCategory.Original,
        CancellationToken cancellationToken = default)
    {
        var blobName = AnimalImage.GetBlobName(animalId, imageId, category);
        return containerManager.OpenReadStreamAsync(blobName, cancellationToken);
    }
}
