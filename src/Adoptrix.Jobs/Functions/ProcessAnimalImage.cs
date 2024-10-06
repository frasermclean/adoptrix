using Adoptrix.Core.Events;
using Adoptrix.Jobs.Services;
using Adoptrix.Logic.Models;
using Adoptrix.Persistence;
using Adoptrix.Persistence.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Jobs.Functions;

public class ProcessAnimalImage(
    ILogger<ProcessAnimalImage> logger,
    AdoptrixDbContext dbContext,
    ImageProcessor imageProcessor,
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
        var animal = await dbContext.Animals.FirstOrDefaultAsync(a => a.Slug == data.AnimalSlug, cancellationToken);
        if (animal is null)
        {
            logger.LogError("Animal with slug {AnimalSlug} not found", data.AnimalSlug);
            throw new InvalidOperationException($"Animal with slug {data.AnimalSlug} not found");
        }

        // process original image
        await using var originalStream
            = await originalImagesContainerManager.OpenReadStreamAsync(data.BlobName, cancellationToken);
        await using var bundle = await imageProcessor.ProcessOriginalAsync(originalStream, cancellationToken);

        // upload processed images
        await UploadProcessedBundleAsync(data.AnimalSlug, data.ImageId, bundle, cancellationToken);

        // update entity in database
        var image = animal.Images.First(image => image.Id == data.ImageId);
        image.IsProcessed = true;
        await dbContext.SaveChangesAsync(cancellationToken);

        // remove original image
        await originalImagesContainerManager.DeleteBlobAsync(data.BlobName, cancellationToken);

        logger.LogInformation("Processed image with ID: {ImageId} for animal with slug: {AnimalId}",
            data.ImageId, data.AnimalSlug);
    }

    private async Task UploadProcessedBundleAsync(string animalSlug, Guid imageId, ImageStreamBundle bundle,
        CancellationToken cancellationToken)
    {
        await Task.WhenAll(
            UploadImageAsync(bundle.PreviewWriteStream, "preview"),
            UploadImageAsync(bundle.ThumbnailWriteStream, "thumb"),
            UploadImageAsync(bundle.FullSizeWriteStream, "full")
        );

        logger.LogInformation("Uploaded processed images for animal with slug: {AnimalId}", animalSlug);
        return;

        async Task UploadImageAsync(Stream stream, string size)
        {
            var blobName = $"{animalSlug}/{imageId}/{size}.webp";
            await animalImagesContainerManager.UploadBlobAsync(blobName, stream, ImageProcessor.OutputContentType,
                cancellationToken);
        }
    }
}
