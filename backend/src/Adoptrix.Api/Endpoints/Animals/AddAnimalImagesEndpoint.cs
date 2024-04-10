using System.Security.Claims;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Mapping;
using Adoptrix.Api.Validators;
using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Events;
using Adoptrix.Domain.Models;
using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Animals;

public static class AddAnimalImagesEndpoint
{
    public static async Task<Results<Ok<AnimalResponse>, NotFound, ValidationProblem>> ExecuteAsync(
        [AsParameters] AddAnimalImagesRequest request,
        AddAnimalRequestValidator validator,
        ClaimsPrincipal claimsPrincipal,
        IAnimalsService animalsService,
        IAnimalImageManager imageManager,
        IEventPublisher eventPublisher,
        CancellationToken cancellationToken)
    {
        // ensure animal exists
        var getResult = await animalsService.GetAsync(request.AnimalId, cancellationToken);
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

        // upload images to blob storage
        var uploadResults = await Task.WhenAll(request.FormFileCollection.Select(async formFile =>
        {
            var image = new AnimalImage
            {
                Description = formFile.Name,
                OriginalFileName = formFile.FileName,
                OriginalContentType = formFile.ContentType,
                UploadedBy = claimsPrincipal.GetUserId()
            };

            var result = await imageManager.UploadImageAsync(request.AnimalId, image.Id, formFile.OpenReadStream(),
                formFile.ContentType, ImageCategory.Original, cancellationToken);

            return result.IsSuccess
                ? Result.Ok(image)
                : Result.Fail(result.Errors);
        }));

        // update database entity
        var addImagesResult = await animalsService.AddImagesAsync(request.AnimalId,
            uploadResults.Where(result => result.IsSuccess).Select(result => result.Value), cancellationToken);
        var animal = addImagesResult.Value;

        // publish domain events
        foreach (var domainEvent in animal.Images.Select(image => new AnimalImageAddedEvent(animal.Id, image.Id)))
        {
            await eventPublisher.PublishDomainEventAsync(domainEvent, cancellationToken);
        }

        return TypedResults.Ok(animal.ToResponse());
    }
}
