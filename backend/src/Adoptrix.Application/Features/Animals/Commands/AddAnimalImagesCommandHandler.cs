using Adoptrix.Application.Errors;
using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Support;
using Adoptrix.Domain.Events;
using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Features.Animals.Commands;

public class AddAnimalImagesCommandHandler(
    ILogger<AddAnimalImagesCommandHandler> logger,
    IAnimalImageManager imageManager,
    IAnimalsRepository animalsRepository,
    IEventPublisher eventPublisher) : IRequestHandler<AddAnimalImagesCommand, Result<Animal>>
{
    public async Task<Result<Animal>> Handle(AddAnimalImagesCommand command, CancellationToken cancellationToken)
    {
        // get animal from database
        var animal = await animalsRepository.GetByIdAsync(command.AnimalId, cancellationToken);
        if (animal is null)
        {
            return new AnimalNotFoundError(command.AnimalId);
        }

        // upload images to blob storage
        var uploadResults = await UploadImagesAsync(command, cancellationToken);
        if (uploadResults.Any(result => result.IsFailed))
        {
            logger.LogError("Failed to upload all images for animal with ID {AnimalId}", command.AnimalId);
            return new ImageUploadError(command.AnimalId);
        }

        // update animal entity with new images
        var images = uploadResults.Select(result => result.Value);
        var updateAnimalResult = await UpdateAnimalEntity(animal, images, cancellationToken);
        if (updateAnimalResult.IsFailed)
        {
            logger.LogError("Failed to update animal with ID {AnimalId}", command.AnimalId);
            return updateAnimalResult;
        }

        // publish notifications
        await Task.WhenAll(uploadResults.Select(result =>
            eventPublisher.PublishAsync(new AnimalImageAddedEvent(command.AnimalId, result.Value.Id),
                cancellationToken)));

        return animal;
    }

    private async Task<Result<AnimalImage>[]> UploadImagesAsync(AddAnimalImagesCommand command,
        CancellationToken cancellationToken) => await Task.WhenAll(command.FileData.Select(async data =>
    {
        var image = new AnimalImage
        {
            Description = data.Description,
            OriginalFileName = data.FileName,
            OriginalContentType = data.ContentType,
            UploadedBy = command.UserId
        };

        var result = await imageManager.UploadImageAsync(command.AnimalId, image.Id, data.Stream,
            data.ContentType, ImageCategory.Original, cancellationToken);

        return result.IsSuccess
            ? Result.Ok(image)
            : Result.Fail(result.Errors);
    }));

    private async Task<Result<Animal>> UpdateAnimalEntity(Animal animal, IEnumerable<AnimalImage> images,
        CancellationToken cancellationToken)
    {
        // update database entity
        animal.Images.AddRange(images);
        await animalsRepository.UpdateAsync(animal, cancellationToken);

        return animal;
    }
}
