using System.Security.Claims;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Api.Validators;
using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Events;
using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public static class AddAnimalImagesEndpoint
{
    public static async Task<Results<Ok<AnimalResponse>, NotFound, BadRequest<ValidationFailedResponse>>> ExecuteAsync(
        Guid animalId,
        IFormFileCollection formFileCollection,
        ClaimsPrincipal claimsPrincipal,
        IAnimalsRepository animalsRepository,
        IAnimalImageManager imageManager,
        IEventPublisher eventPublisher,
        ImageFormFileValidator imageFormFileValidator,
        CancellationToken cancellationToken)
    {
        // ensure animal exists
        var getResult = await animalsRepository.GetAsync(animalId, cancellationToken);
        if (getResult.IsFailed)
        {
            return TypedResults.NotFound();
        }

        // validate images
        var validationFailures = formFileCollection.Select(imageFormFileValidator.Validate)
            .Where(result => !result.IsValid)
            .ToArray();
        if (validationFailures.Length > 0)
        {
            return TypedResults.BadRequest(new ValidationFailedResponse
            {
                Message = "One or more images are invalid",
                Errors = validationFailures.Select(result =>
                {
                    var failure = result.Errors.First();
                    return $"{failure.PropertyName}: {failure.ErrorMessage}";
                })
            });
        }

        var animal = getResult.Value;
        var userId = claimsPrincipal.GetUserId();

        // add images to animal
        var addImageResults = formFileCollection
            .Select(formFile => animal.AddImage(formFile.FileName, formFile.ContentType, formFile.Name, userId))
            .ToArray();
        if (addImageResults.Any(result => result.IsFailed))
        {
            return TypedResults.BadRequest(new ValidationFailedResponse
            {
                Message = "Duplicate image found",
                Errors = addImageResults.Where(result => result.IsFailed)
                    .Select(result => result.GetFirstErrorMessage())
            });
        }

        // process images
        var processImageResults = await Task.WhenAll(formFileCollection.Zip(addImageResults,
            (formFile, addImageResult) =>
            {
                var imageId = addImageResult.Value.Id;
                return ProcessImageAsync(formFile, imageManager, animalsRepository, eventPublisher, animal, imageId,
                    cancellationToken);
            }));

        return processImageResults.All(result => result.IsSuccess)
            ? TypedResults.Ok(animal.ToResponse())
            : TypedResults.BadRequest(new ValidationFailedResponse
            {
                Message = "Failed to process all images",
                Errors = processImageResults.Select(result => result.GetFirstErrorMessage())
            });
    }

    private static async Task<Result> ProcessImageAsync(IFormFile formFile, IAnimalImageManager imageManager,
        IAnimalsRepository animalsRepository, IEventPublisher eventPublisher, Animal animal, Guid imageId,
        CancellationToken cancellationToken)
    {
        await using var fileStream = formFile.OpenReadStream();

        // upload the original image to blob storage
        var uploadResult = await imageManager.UploadImageAsync(animal.Id, imageId, fileStream, formFile.ContentType,
            ImageCategory.Original, cancellationToken);
        if (uploadResult.IsFailed)
        {
            return uploadResult;
        }

        // update animal in the database
        var updateDatabaseResult = await animalsRepository.UpdateAsync(animal, cancellationToken);
        if (updateDatabaseResult.IsFailed)
        {
            return updateDatabaseResult.ToResult();
        }

        // publish domain event
        var domainEvent = new AnimalImageAddedEvent(animal.Id, imageId);
        return await eventPublisher.PublishDomainEventAsync(domainEvent, cancellationToken);
    }
}
