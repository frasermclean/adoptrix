using Adoptrix.Core;
using Adoptrix.Core.Events;
using Adoptrix.Persistence;
using Adoptrix.Persistence.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Api.Endpoints.Animals.Images;

public class AddAnimalImagesEndpoint(
    AdoptrixDbContext dbContext,
    [FromKeyedServices(BlobContainerNames.OriginalImages)]
    IBlobContainerManager blobContainerManager,
    IEventPublisher eventPublisher)
    : Endpoint<AddAnimalImagesRequest, Results<Ok<AnimalResponse>, NotFound, ErrorResponse>, AnimalResponseMapper>
{
    public override void Configure()
    {
        Post("animals/{animalId:guid}/images");
        AllowFileUploads(true);
    }

    public override async Task<Results<Ok<AnimalResponse>, NotFound, ErrorResponse>> ExecuteAsync(
        AddAnimalImagesRequest request, CancellationToken cancellationToken)
    {
        // ensure animal exists in database
        var animal = await dbContext.Animals.Where(animal => animal.Id == request.AnimalId)
            .Include(animal => animal.Breed)
            .ThenInclude(breed => breed.Species)
            .FirstOrDefaultAsync(cancellationToken);

        if (animal is null)
        {
            Logger.LogError("Could not add images as animal with ID {AnimalId} not found", request.AnimalId);
            return TypedResults.NotFound();
        }

        // upload original images to blob storage
        var images = await FormFileSectionsAsync(cancellationToken)
            .SelectAwait(async section =>
            {
                var image = AnimalImage.Create(animal.Slug, section!.Name, section.FileName,
                    section.Section.ContentType!);

                await blobContainerManager.UploadBlobAsync(image.OriginalBlobName, section.FileStream!,
                    image.OriginalContentType, cancellationToken);

                Logger.LogInformation("Uploaded original image {BlobName}", image.OriginalBlobName);

                return image;
            })
            .ToListAsync(cancellationToken);

        // update animal entity with new images
        animal.Images.AddRange(images);
        await dbContext.SaveChangesAsync(cancellationToken);
        Logger.LogInformation("Added {Count} original images for animal with ID: {AnimalId}",
            images.Count, request.AnimalId);

        // publish events for each image added
        foreach (var animalImageAddedEvent in images.Select(image =>
                     new AnimalImageAddedEvent(animal.Slug, image.Id, image.OriginalBlobName)))
        {
            await eventPublisher.PublishAsync(animalImageAddedEvent, cancellationToken);
        }

        return TypedResults.Ok(Map.FromEntity(animal));
    }
}
