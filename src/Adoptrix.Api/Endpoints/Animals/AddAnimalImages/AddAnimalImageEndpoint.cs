using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;


namespace Adoptrix.Api.Endpoints.Animals.AddAnimalImages;

public class AddAnimalImagesEndpoint(IAnimalsRepository repository, IAnimalImageManager imageManager,
        ILogger<AddAnimalImagesEndpoint> logger)
    : Endpoint<AddAnimalImagesRequest, Results<Ok, NotFound>>
{
    public override void Configure()
    {
        Post("animals/{id}/images");
        AllowFileUploads(true);
    }

    public override async Task<Results<Ok, NotFound>> ExecuteAsync(
        AddAnimalImagesRequest request, CancellationToken cancellationToken)
    {
        // check if the animal exists
        var result = await repository.GetAsync(request.Id, cancellationToken);
        if (result.IsFailed)
        {
            return TypedResults.NotFound();
        }

        var animal = result.Value;

        // iterate over the form file sections and upload each image
        await foreach (var section in FormFileSectionsAsync(cancellationToken))
        {
            if (section is null) continue;

            var contentType = section.Section.ContentType ?? "application/octet-stream";
            var fileName = imageManager.GenerateFileName(contentType, section.FileName);

            // check if the image already exists
            if (animal.ImageExists(fileName))
            {
                logger.LogWarning(
                    "Duplicate image detected with file name {FileName}, original file name: {OriginalFileName}",
                    fileName, section.FileName);
                continue;
            }

            // upload the image to blob storage
            await imageManager.UploadImageAsync($"{animal.Id}/{fileName}", section.Section.Body, contentType,
                cancellationToken);

            animal.AddImage(fileName, section.Name, section.FileName);
        }

        await repository.UpdateAsync(animal, cancellationToken);
        return TypedResults.Ok();
    }
}