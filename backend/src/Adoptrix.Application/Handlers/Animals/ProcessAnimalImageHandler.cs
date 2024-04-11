using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Handlers.Animals;

public class ProcessAnimalImageHandler(
    ILogger<ProcessAnimalImageHandler> logger,
    IAnimalImageManager imageManager,
    IImageProcessor imageProcessor,
    IAnimalsRepository animalsRepository)
    : IRequestHandler<ProcessAnimalImageRequest, Result>
{
    public async Task<Result> Handle(ProcessAnimalImageRequest request, CancellationToken cancellationToken = default)
    {
        // process original image
        await using var originalReadStream = await imageManager.GetImageReadStreamAsync(request.AnimalId,
            request.ImageId, cancellationToken: cancellationToken);
        await using var bundle = await imageProcessor.ProcessOriginalAsync(originalReadStream, cancellationToken);

        // upload processed images
        await UploadImagesAsync(request.AnimalId, request.ImageId, bundle);

        // update entity in database
        var updateResult = await SetImageProcessedAsync(request.AnimalId, request.ImageId, cancellationToken);

        if (updateResult.IsSuccess)
        {
            logger.LogInformation("Processed image with ID: {ImageId} for animal with ID: {AnimalId}",
                request.ImageId, request.AnimalId);
        }

        return updateResult;
    }

    private async Task UploadImagesAsync(Guid animalId, Guid imageId, ImageStreamBundle bundle)
    {
        // upload processed images
        await Task.WhenAll(
            imageManager.UploadImageAsync(animalId, imageId, bundle.ThumbnailWriteStream,
                ImageProcessor.OutputContentType, ImageCategory.Thumbnail),
            imageManager.UploadImageAsync(animalId, imageId, bundle.PreviewWriteStream,
                ImageProcessor.OutputContentType, ImageCategory.Preview),
            imageManager.UploadImageAsync(animalId, imageId, bundle.FullSizeWriteStream,
                ImageProcessor.OutputContentType, ImageCategory.FullSize)
        );

        logger.LogInformation("Uploaded processed images for animal with ID: {AnimalId}", animalId);
    }

    private async Task<Result> SetImageProcessedAsync(Guid animalId, Guid imageId,
        CancellationToken cancellationToken = default)
    {
        var animal = await animalsRepository.GetByIdAsync(animalId, cancellationToken);
        if (animal is null)
        {
            return new AnimalNotFoundError(animalId);
        }

        var image = animal.Images.First(image => image.Id == imageId);
        image.IsProcessed = true;

        await animalsRepository.UpdateAsync(animal, cancellationToken);
        return Result.Ok();
    }
}
