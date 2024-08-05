using Adoptrix.Core.Events;
using Adoptrix.Jobs.Services;
using Adoptrix.Persistence;
using Adoptrix.Persistence.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Jobs.Functions;

public class ProcessAnimalImage(
    ILogger<ProcessAnimalImage> logger,
    IAnimalsRepository animalsRepository,
    IImageProcessor imageProcessor,
    [FromKeyedServices(BlobContainerNames.OriginalImages)]
    IBlobContainerManager originalImagesContainerManager,
    [FromKeyedServices(BlobContainerNames.AnimalImages)]
    IBlobContainerManager animalImagesContainerManager)
{
    [Function(nameof(ProcessAnimalImage))]
    public async Task ExecuteAsync([QueueTrigger(QueueNames.AnimalImageAdded)] AnimalImageAddedEvent data,
        CancellationToken cancellationToken = default)
    {
        // ensure animal exists in database
        var animal = await animalsRepository.GetAsync(data.AnimalId, cancellationToken);
        if (animal is null)
        {
            throw new InvalidOperationException($"Could not find animal with ID {data.AnimalId}");
        }

        // process original image
        await using var originalStream
            = await originalImagesContainerManager.OpenReadStreamAsync(data.BlobName, cancellationToken);
        await using var bundle = await imageProcessor.ProcessOriginalAsync(originalStream, cancellationToken);

        // upload processed images
        await UploadProcessedBundleAsync(data.AnimalId, data.ImageId, bundle, cancellationToken);

        // update entity in database
        var image = animal.Images.First(image => image.Id == data.ImageId);
        image.IsProcessed = true;
        await animalsRepository.SaveChangesAsync(cancellationToken);

        // remove original image
        await originalImagesContainerManager.DeleteBlobAsync(data.BlobName, cancellationToken);

        logger.LogInformation("Processed image with ID: {ImageId} for animal with ID: {AnimalId}",
            data.ImageId, data.AnimalId);
    }

    private async Task UploadProcessedBundleAsync(int animalId, int imageId, ImageStreamBundle bundle,
        CancellationToken cancellationToken)
    {
        await Task.WhenAll(
            UploadImageAsync(bundle.PreviewWriteStream, "preview"),
            UploadImageAsync(bundle.ThumbnailWriteStream, "thumb"),
            UploadImageAsync(bundle.FullSizeWriteStream, "full")
        );

        logger.LogInformation("Uploaded processed images for animal with ID: {AnimalId}", animalId);
        return;

        async Task UploadImageAsync(Stream stream, string size)
        {
            var blobName = $"{animalId}/{imageId}/{size}.webp";
            await animalImagesContainerManager.UploadBlobAsync(blobName, stream, ImageProcessor.OutputContentType,
                cancellationToken);
        }
    }
}
