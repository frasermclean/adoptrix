using Adoptrix.Application.Models;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Events;
using Adoptrix.Storage;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Jobs.Functions;

public class ProcessNewAnimalImage(
    ILogger<ProcessNewAnimalImage> logger,
    IAnimalImageManager animalImageManager,
    IImageProcessor imageProcessor,
    IAnimalsService animalsService)
{
    [Function(nameof(ProcessNewAnimalImage))]
    public async Task Run([QueueTrigger(QueueNames.AnimalImageAdded)] AnimalImageAddedEvent eventData)
    {
        var (animalId, imageId) = eventData;

        // process original image
        await using var originalReadStream = await animalImageManager.GetImageReadStreamAsync(animalId, imageId);
        await using var bundle = await imageProcessor.ProcessOriginalAsync(originalReadStream);

        // upload processed images
        await UploadImagesAsync(animalId, imageId, bundle);

        // update entity in database
        await UpdateEntityAsync(animalId, imageId);

        logger.LogInformation("Processed image with ID: {ImageId} for animal with ID: {AnimalId}", imageId, animalId);
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

    private async Task UpdateEntityAsync(Guid animalId, Guid imageId)
    {
        var getResult = await animalsService.GetAsync(animalId);

        var animal = getResult.IsSuccess
            ? getResult.Value
            : throw new InvalidOperationException($"Animal with ID {animalId} not found");

        var image = animal.Images.First(image => image.Id == imageId);
        image.IsProcessed = true;

        await animalsService.UpdateAsync(animal);
    }
}
