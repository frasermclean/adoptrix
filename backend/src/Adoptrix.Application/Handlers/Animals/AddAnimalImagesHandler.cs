using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Events;
using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Handlers.Animals;

public class AddAnimalImagesHandler(
    ILogger<AddAnimalImagesHandler> logger,
    ISender sender,
    IAnimalImageManager imageManager,
    IAnimalsRepository animalsRepository,
    IEventPublisher eventPublisher) : IRequestHandler<AddAnimalImagesRequest, Result<Animal>>
{
    public async Task<Result<Animal>> Handle(AddAnimalImagesRequest request, CancellationToken cancellationToken)
    {
        // upload images to blob storage
        var uploadResults = await UploadImagesAsync(request, cancellationToken);
        if (uploadResults.Any(result => result.IsFailed))
        {
            logger.LogError("Failed to upload all images for animal with ID {AnimalId}", request.AnimalId);
            return new ImageUploadError(request.AnimalId);
        }

        // update animal entity with new images
        var updateAnimalResult = await UpdateAnimalEntity(request.AnimalId,
            uploadResults.Select(result => result.Value), cancellationToken);
        if (updateAnimalResult.IsFailed)
        {
            logger.LogError("Failed to update animal with ID {AnimalId}", request.AnimalId);
            return updateAnimalResult;
        }

        var animal = updateAnimalResult.Value;

        // publish domain events
        foreach (var domainEvent in animal.Images.Select(image => new AnimalImageAddedEvent(animal.Id, image.Id)))
        {
            await eventPublisher.PublishDomainEventAsync(domainEvent, cancellationToken);
        }

        return animal;
    }

    private async Task<Result<AnimalImage>[]> UploadImagesAsync(AddAnimalImagesRequest request,
        CancellationToken cancellationToken) => await Task.WhenAll(request.FileData.Select(async data =>
    {
        var image = new AnimalImage
        {
            Description = data.Description,
            OriginalFileName = data.FileName,
            OriginalContentType = data.ContentType,
            UploadedBy = request.UserId
        };

        var result = await imageManager.UploadImageAsync(request.AnimalId, image.Id, data.Stream,
            data.ContentType, ImageCategory.Original, cancellationToken);

        return result.IsSuccess
            ? Result.Ok(image)
            : Result.Fail(result.Errors);
    }));

    private async Task<Result<Animal>> UpdateAnimalEntity(Guid animalId, IEnumerable<AnimalImage> images,
        CancellationToken cancellationToken)
    {
        // get animal from database
        var getAnimalResult = await sender.Send(new GetAnimalRequest(animalId), cancellationToken);
        if (getAnimalResult.IsFailed)
        {
            return getAnimalResult;
        }

        // update database entity
        var animal = getAnimalResult.Value;
        animal.Images.AddRange(images);
        await animalsRepository.UpdateAsync(animal, cancellationToken);

        return animal;
    }
}
