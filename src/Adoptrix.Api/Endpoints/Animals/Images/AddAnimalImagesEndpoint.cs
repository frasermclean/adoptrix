using Adoptrix.Core;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Core.Events;
using Adoptrix.Persistence;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using AnimalMapper = Adoptrix.Api.Mapping.AnimalMapper;

namespace Adoptrix.Api.Endpoints.Animals.Images;

[HttpPost("animals/{animalId:guid}/images"), AllowFileUploads(true)]
public class AddAnimalImagesEndpoint(
    IAnimalsRepository animalsRepository,
    IEventPublisher eventPublisher,
    [FromKeyedServices(BlobContainerNames.OriginalImages)]
    IBlobContainerManager containerManager)
    : Endpoint<AddAnimalImagesRequest, Results<Ok<AnimalResponse>, NotFound, ErrorResponse>>
{
    public override async Task<Results<Ok<AnimalResponse>, NotFound, ErrorResponse>> ExecuteAsync(
        AddAnimalImagesRequest request, CancellationToken cancellationToken)
    {
        // ensure animal exists in database
        var animal = await animalsRepository.GetByIdAsync(request.AnimalId, cancellationToken);
        if (animal is null)
        {
            return TypedResults.NotFound();
        }

        // upload original images to blob storage
        var images = await UploadOriginalsAsync(animal.Id, request.UserId, cancellationToken);

        // update animal entity with new images
        animal.Images.AddRange(images);
        await animalsRepository.SaveChangesAsync(cancellationToken);

        // publish events for each image added
        foreach (var @event in images.Select(image =>
                     new AnimalImageAddedEvent(animal.Id, image.Id, GetBlobName(animal.Id, image.OriginalFileName))))
        {
            await eventPublisher.PublishAsync(@event, cancellationToken);
        }

        return TypedResults.Ok(AnimalMapper.ToResponse(animal));
    }

    private async Task<List<AnimalImage>> UploadOriginalsAsync(Guid animalId, Guid userId,
        CancellationToken cancellationToken)
    {
        List<AnimalImage> images = [];
        await foreach (var section in FormFileSectionsAsync(cancellationToken))
        {
            var image = new AnimalImage
            {
                Description = section!.Name,
                OriginalFileName = section.FileName,
                OriginalContentType = section.Section.ContentType!,
                UploadedBy = userId
            };

            var blobName = GetBlobName(animalId, section.FileName);
            await containerManager.UploadBlobAsync(blobName, section.FileStream!, section.Section.ContentType!,
                cancellationToken);

            Logger.LogInformation("Uploaded original image {BlobName} for animal {AnimalId}", blobName, animalId);

            images.Add(image);
        }

        return images;
    }

    private static string GetBlobName(Guid animalId, string fileName) => $"{animalId}/{fileName}";
}
