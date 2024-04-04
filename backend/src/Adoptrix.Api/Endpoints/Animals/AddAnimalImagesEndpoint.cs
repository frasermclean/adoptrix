using System.Security.Claims;
using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Api.Validators;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Events;
using Adoptrix.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public static class AddAnimalImagesEndpoint
{
    public static async Task<Results<Ok<AnimalResponse>, NotFound, ValidationProblem>> ExecuteAsync(
        [AsParameters] AddAnimalImagesRequest request,
        AddAnimalRequestValidator validator,
        ClaimsPrincipal claimsPrincipal,
        IAnimalsRepository animalsRepository,
        IAnimalImageManager imageManager,
        IEventPublisher eventPublisher,
        CancellationToken cancellationToken)
    {
        // ensure animal exists
        var getResult = await animalsRepository.GetAsync(request.AnimalId, cancellationToken);
        if (getResult.IsFailed)
        {
            return TypedResults.NotFound();
        }

        // validate request
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var animal = getResult.Value;
        var userId = claimsPrincipal.GetUserId();

        // add images to animal
        foreach (var formFile in request.FormFileCollection)
        {
            var image = animal.AddImage(formFile.FileName, formFile.ContentType, formFile.Name, userId);
            await ProcessImageAsync(formFile, imageManager, animalsRepository, eventPublisher, animal, image.Id,
                cancellationToken);
        }

        return TypedResults.Ok(animal.ToResponse());
    }

    private static async Task ProcessImageAsync(IFormFile formFile, IAnimalImageManager imageManager,
        IAnimalsRepository animalsRepository, IEventPublisher eventPublisher, Animal animal, Guid imageId,
        CancellationToken cancellationToken)
    {
        await using var fileStream = formFile.OpenReadStream();

        // upload the original image to blob storage
        await imageManager.UploadImageAsync(animal.Id, imageId, fileStream, formFile.ContentType,
            cancellationToken: cancellationToken);

        // update animal in the database
        await animalsRepository.UpdateAsync(animal, cancellationToken);

        // publish domain event
        var domainEvent = new AnimalImageAddedEvent(animal.Id, imageId);
        await eventPublisher.PublishDomainEventAsync(domainEvent, cancellationToken);
    }
}
