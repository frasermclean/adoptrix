﻿using Adoptrix.Application.Mapping;
using Adoptrix.Domain.Commands.Animals;
using Adoptrix.Domain.Contracts.Requests;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Errors;
using Adoptrix.Domain.Events;
using Adoptrix.Domain.Models;
using Adoptrix.Domain.Services;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Services;

public class AnimalsService(
    ILogger<AnimalsService> logger,
    IAnimalsRepository animalsRepository,
    IBreedsRepository breedsRepository,
    IAnimalImageManager imageManager,
    IEventPublisher eventPublisher) : IAnimalsService
{
    private readonly Uri containerUri = imageManager.ContainerUri;

    public async Task<IEnumerable<AnimalMatch>> SearchAsync(SearchAnimalsRequest? request,
        CancellationToken cancellationToken)
    {
        var matches = await animalsRepository.SearchAsync(request ?? new SearchAnimalsRequest(), cancellationToken);
        var matchesList = matches as List<AnimalMatch> ?? matches.ToList();

        foreach (var match in matchesList)
        {
            if (match.Image is null)
            {
                continue;
            }

            SetImageUrls(match.Image, match.Id, containerUri);
        }

        return matchesList;
    }

    public async Task<Result<AnimalResponse>> GetAsync(Guid animalId, CancellationToken cancellationToken)
    {
        var animal = await animalsRepository.GetByIdAsync(animalId, cancellationToken);
        if (animal is null)
        {
            return new AnimalNotFoundError(animalId);
        }

        var response = animal.ToResponse();

        foreach (var imageResponse in response.Images)
        {
            if (!imageResponse.IsProcessed)
            {
                continue;
            }

            SetImageUrls(imageResponse, animalId, containerUri);
        }

        return response;
    }

    public async Task<Result<AnimalResponse>> AddAsync(AddAnimalCommand command, CancellationToken cancellationToken)
    {
        var breed = await breedsRepository.GetByIdAsync(command.BreedId, cancellationToken);
        if (breed is null)
        {
            logger.LogError("Breed with ID {BreedId} was not found", command.BreedId);
            return new BreedNotFoundError(command.BreedId);
        }

        var animal = new Animal
        {
            Name = command.Name,
            Description = command.Description,
            Breed = breed,
            Sex = command.Sex,
            DateOfBirth = command.DateOfBirth,
            CreatedBy = command.UserId
        };

        await animalsRepository.AddAsync(animal, cancellationToken);

        logger.LogInformation("Animal with ID {AnimalId} was added successfully", animal.Id);

        return animal.ToResponse();
    }

    public async Task<Result<AnimalResponse>> UpdateAsync(UpdateAnimalCommand command,
        CancellationToken cancellationToken = default)
    {
        var animal = await animalsRepository.GetByIdAsync(command.AnimalId, cancellationToken);
        if (animal is null)
        {
            logger.LogError("Animal with ID {AnimalId} was not found", command.AnimalId);
            return new AnimalNotFoundError(command.AnimalId);
        }

        var breed = await breedsRepository.GetByIdAsync(command.BreedId, cancellationToken);
        if (breed is null)
        {
            logger.LogError("Breed with ID {BreedId} was not found", command.BreedId);
            return new BreedNotFoundError(command.BreedId);
        }

        animal.Name = command.Name;
        animal.Description = command.Description;
        animal.Breed = breed;
        animal.Sex = command.Sex;
        animal.DateOfBirth = command.DateOfBirth;

        await animalsRepository.UpdateAsync(animal, cancellationToken);

        logger.LogInformation("Updated animal with ID: {AnimalId}", animal.Id);

        return animal.ToResponse();
    }

    public async Task<Result> DeleteAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var animal = await animalsRepository.GetByIdAsync(animalId, cancellationToken);
        if (animal is null)
        {
            logger.LogError("Could not find animal with ID: {AnimalId} to delete", animalId);
            return new AnimalNotFoundError(animalId);
        }

        await animalsRepository.DeleteAsync(animal, cancellationToken);
        logger.LogInformation("Deleted animal with ID: {AnimalId}", animalId);

        // publish domain event
        await eventPublisher.PublishAsync(new AnimalDeletedEvent(animalId), cancellationToken);

        return Result.Ok();
    }

    public async Task<Result<AnimalResponse>> AddImagesAsync(AddAnimalImagesCommand command,
        CancellationToken cancellationToken = default)
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
        animal.Images.AddRange(images);
        await animalsRepository.UpdateAsync(animal, cancellationToken);


        // publish notifications
        await Task.WhenAll(uploadResults.Select(result =>
            eventPublisher.PublishAsync(new AnimalImageAddedEvent(command.AnimalId, result.Value.Id),
                cancellationToken)));

        return animal.ToResponse();
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
            data.ContentType, AnimalImageCategory.Original, cancellationToken);

        return result.IsSuccess
            ? Result.Ok(image)
            : Result.Fail(result.Errors);
    }));

    private static void SetImageUrls(AnimalImageResponse response, Guid animalId, Uri containerUri)
    {
        var previewBlobName = AnimalImage.GetBlobName(animalId, response.Id, AnimalImageCategory.Preview);
        var thumbnailBlobName = AnimalImage.GetBlobName(animalId, response.Id, AnimalImageCategory.Thumbnail);
        var fullSizeBlobName = AnimalImage.GetBlobName(animalId, response.Id, AnimalImageCategory.FullSize);

        response.PreviewUrl = $"{containerUri}/{previewBlobName}";
        response.ThumbnailUrl = $"{containerUri}/{thumbnailBlobName}";
        response.FullSizeUrl = $"{containerUri}/{fullSizeBlobName}";
    }
}
