using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Core;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Extensions;
using FastEndpoints;
using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Animals.Images;

public class AddAnimalImagesEndpoint(IAnimalsRepository animalsRepository, IAnimalImageManager animalImageManager)
    : EndpointWithoutRequest<Results<Ok<AnimalResponse>, NotFound, BadRequest>>
{
    public override void Configure()
    {
        Post("animals/{animalId:guid}/images");
        AllowFileUploads(true);
    }

    public override async Task<Results<Ok<AnimalResponse>, NotFound, BadRequest>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var animalId = Route<Guid>("animalId");
        var userId = User.GetUserId();

        // ensure animal exists in database
        var animal = await animalsRepository.GetByIdAsync(animalId, cancellationToken);
        if (animal is null)
        {
            return TypedResults.NotFound();
        }

        List<Result<AnimalImage>> results = [];
        await foreach (var section in FormFileSectionsAsync(cancellationToken))
        {
            if (section is null)
            {
                Logger.LogWarning("Skipping null section");
                continue;
            }

            if (section.Section.ContentType is null || section.FileStream is null)
            {
                Logger.LogWarning("Skipping section {Name} as it has no content type or file stream", section.Name);
                continue;
            }

            var result = await animalImageManager.UploadOriginalAsync(animalId, userId, section.FileName, section.Name,
                section.Section.ContentType, section.FileStream, cancellationToken);

            results.Add(result);
        }

        if (results.Any(result => result.IsFailed))
        {
            return TypedResults.BadRequest();
        }

        var images = results.Select(result => result.Value).ToArray();
        var addImagesResult = await animalImageManager.AddImagesToAnimalAsync(animal, images, cancellationToken);

        return addImagesResult.IsSuccess
            ? TypedResults.Ok(addImagesResult.Value)
            : TypedResults.BadRequest();
    }
}
