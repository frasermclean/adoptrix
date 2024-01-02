using Adoptrix.Application.Models;
using Adoptrix.Application.Services;
using Adoptrix.Domain;
using Adoptrix.Domain.Events;
using Adoptrix.Infrastructure.Storage;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Jobs.Functions;

public class ProcessNewAnimalImage(
    ILogger<ProcessNewAnimalImage> logger,
    IAnimalImageManager animalImageManager,
    IImageProcessor imageProcessor)
{
    [Function(nameof(ProcessNewAnimalImage))]
    public async Task Run([QueueTrigger(QueueNames.AnimalImageAdded)] AnimalImageAddedEvent eventData)
    {
        // resolve scoped services
        var (animalId, imageId) = eventData;

        // get original image stream
        await using var originalReadStream = await animalImageManager.GetImageReadStreamAsync(animalId, imageId);

        // process original image
        await using var bundle = await imageProcessor.ProcessOriginalAsync(originalReadStream);

        // upload processed images
        await UploadImagesAsync(animalId, imageId, bundle);
    }

    private async Task UploadImagesAsync(Guid animalId, Guid imageId, ImageStreamBundle bundle)
    {
        // upload processed images
        await Task.WhenAll(
            animalImageManager.UploadImageAsync(animalId, imageId, bundle.ThumbnailWriteStream,
                ImageProcessor.OutputContentType, ImageCategory.Thumbnail),
            animalImageManager.UploadImageAsync(animalId, imageId, bundle.PreviewWriteStream,
                ImageProcessor.OutputContentType, ImageCategory.Preview),
            animalImageManager.UploadImageAsync(animalId, imageId, bundle.FullSizeWriteStream,
                ImageProcessor.OutputContentType, ImageCategory.FullSize)
        );

        logger.LogInformation("Uploaded processed images for animal with ID: {AnimalId}", animalId);
    }
}