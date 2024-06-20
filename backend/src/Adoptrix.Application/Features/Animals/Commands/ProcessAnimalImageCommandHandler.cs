using Adoptrix.Application.Errors;
using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Support;
using Adoptrix.Domain.Commands.Animals;
using Adoptrix.Domain.Models;
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
        // ensure animal exists in database
        var animal = await animalsRepository.GetByIdAsync(command.AnimalId, cancellationToken);
        if (animal is null)
        {
            logger.LogError("Could not find animal with ID {AnimalId}, will not process command", command.AnimalId);
            return new AnimalNotFoundError(command.AnimalId);
        }

        // process original image
        await using var originalReadStream = await imageManager.GetImageReadStreamAsync(command.AnimalId,
            command.ImageId, cancellationToken: cancellationToken);
        await using var bundle = await imageProcessor.ProcessOriginalAsync(originalReadStream, cancellationToken);

        // upload processed images
        await UploadImagesAsync(command.AnimalId, command.ImageId, bundle);

        // update entity in database
        var updateResult = await SetImageProcessedAsync(animal, command.ImageId, cancellationToken);

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
                ImageProcessor.OutputContentType, AnimalImageCategory.Thumbnail),
            imageManager.UploadImageAsync(animalId, imageId, bundle.PreviewWriteStream,
                ImageProcessor.OutputContentType, AnimalImageCategory.Preview),
            imageManager.UploadImageAsync(animalId, imageId, bundle.FullSizeWriteStream,
                ImageProcessor.OutputContentType, AnimalImageCategory.FullSize)
        );

        logger.LogInformation("Uploaded processed images for animal with ID: {AnimalId}", animalId);
    }

    private async Task<Result> SetImageProcessedAsync(Animal animal, Guid imageId, CancellationToken cancellationToken)
    {
        var image = animal.Images.FirstOrDefault(image => image.Id == imageId);
        if (image is null)
        {
            return new AnimalImageNotFoundError(imageId, animal.Id);
        }

        image.IsProcessed = true;

        await animalsRepository.UpdateAsync(animal, cancellationToken);
        return Result.Ok();
    }
}
