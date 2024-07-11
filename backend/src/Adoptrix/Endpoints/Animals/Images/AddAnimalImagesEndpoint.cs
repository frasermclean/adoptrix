using Adoptrix.Core;
using Adoptrix.Core.Abstractions;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Core.Events;
using Adoptrix.Mapping;
using Adoptrix.Storage;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Animals.Images;

[HttpPost("animals/{animalId:guid}/images"), AllowFileUploads(true)]
public class AddAnimalImagesEndpoint(
    IAnimalsRepository animalsRepository,
    IEventPublisher eventPublisher,
    [FromKeyedServices(BlobContainerNames.AnimalImages)]
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
        var images = await UploadOriginalsAsync(request.AnimalId, request.UserId, cancellationToken);

        // update animal entity with new images
        animal.Images.AddRange(images);
        await animalsRepository.SaveChangesAsync(cancellationToken);

        // publish events for each image added
        foreach (var image in images)
        {
            await eventPublisher.PublishAsync(new AnimalImageAddedEvent(animal.Id, image.Id), cancellationToken);
        }

        return TypedResults.Ok(animal.ToResponse());
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
                UploadedBy = userId,
                AnimalId = animalId
            };

            var blobName = AnimalImage.GetBlobName(animalId, image.Id, AnimalImageCategory.Original);
            await containerManager.UploadBlobAsync(blobName, section.FileStream!, section.Section.ContentType!,
                cancellationToken);

            Logger.LogInformation("Uploaded original image {BlobName} for animal {AnimalId}", blobName, animalId);

            images.Add(image);
        }

        return images;
    }
}
