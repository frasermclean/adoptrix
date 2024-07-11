using Adoptrix.Application;
using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Core;
using Adoptrix.Core.Events;
using Adoptrix.Jobs.Services;
using Adoptrix.Storage;
using FluentResults;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Jobs.Functions;

public class ProcessAnimalImage(
    ILogger<ProcessAnimalImage> logger,
    IAnimalsRepository animalsRepository,
    IImageProcessor imageProcessor,
    [FromKeyedServices(BlobContainerNames.AnimalImages)]
    IBlobContainerManager containerManager)
{
    [Function(nameof(ProcessAnimalImage))]
    public async Task ExecuteAsync([QueueTrigger(QueueNames.AnimalImageAdded)] AnimalImageAddedEvent data,
        CancellationToken cancellationToken = default)
    {
        // ensure animal exists in database
        var animal = await animalsRepository.GetByIdAsync(data.AnimalId, cancellationToken);
        if (animal is null)
        {
            throw new InvalidOperationException($"Could not find animal with ID {data.AnimalId}");
        }

        // process original image
        await using var originalReadStream =
            await GetImageReadStreamAsync(data.AnimalId, data.ImageId, AnimalImageCategory.Original,
                cancellationToken);
        await using var bundle = await imageProcessor.ProcessOriginalAsync(originalReadStream, cancellationToken);

        // upload processed images
        await UploadProcessedBundleAsync(data.AnimalId, data.ImageId, bundle, cancellationToken);

        // update entity in database
        var image = animal.Images.First(image => image.Id == data.ImageId);
        image.IsProcessed = true;
        await animalsRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Processed image with ID: {ImageId} for animal with ID: {AnimalId}",
            data.ImageId, data.AnimalId);
    }

    private Task<Stream> GetImageReadStreamAsync(Guid animalId, Guid imageId,
        AnimalImageCategory category = AnimalImageCategory.Original,
        CancellationToken cancellationToken = default)
    {
        var blobName = AnimalImage.GetBlobName(animalId, imageId, category);
        return containerManager.OpenReadStreamAsync(blobName, cancellationToken);
    }

    private async Task UploadProcessedBundleAsync(Guid animalId, Guid imageId, ImageStreamBundle bundle,
        CancellationToken cancellationToken)
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
        await containerManager.UploadBlobAsync(blobName, imageStream, contentType, cancellationToken);

        return Result.Ok();
    }
}
