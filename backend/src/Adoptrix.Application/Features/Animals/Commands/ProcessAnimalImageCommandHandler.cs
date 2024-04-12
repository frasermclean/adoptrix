using Adoptrix.Application.Errors;
using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Support;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Features.Animals.Commands;

public class ProcessAnimalImageCommandHandler(
    ILogger<ProcessAnimalImageCommandHandler> logger,
    IAnimalImageManager imageManager,
    IImageProcessor imageProcessor,
    IAnimalsRepository animalsRepository)
    : IRequestHandler<ProcessAnimalImageCommand, Result>
{
    public async Task<Result> Handle(ProcessAnimalImageCommand command, CancellationToken cancellationToken = default)
    {
        // process original image
        await using var originalReadStream = await imageManager.GetImageReadStreamAsync(command.AnimalId,
            command.ImageId, cancellationToken: cancellationToken);
        await using var bundle = await imageProcessor.ProcessOriginalAsync(originalReadStream, cancellationToken);

        // upload processed images
        await UploadImagesAsync(command.AnimalId, command.ImageId, bundle);

        // update entity in database
        var updateResult = await SetImageProcessedAsync(command.AnimalId, command.ImageId, cancellationToken);

        if (updateResult.IsSuccess)
        {
            logger.LogInformation("Processed image with ID: {ImageId} for animal with ID: {AnimalId}",
                command.ImageId, command.AnimalId);
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

        var image = animal.Images.FirstOrDefault(image => image.Id == imageId);
        if (image is null)
        {
            return new AnimalImageNotFoundError(imageId, animalId);
        }
        image.IsProcessed = true;

        await animalsRepository.UpdateAsync(animal, cancellationToken);
        return Result.Ok();
    }
}
