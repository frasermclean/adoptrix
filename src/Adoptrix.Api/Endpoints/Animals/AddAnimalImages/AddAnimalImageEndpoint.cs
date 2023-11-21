using Adoptrix.Application.Services;
using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Animals.AddAnimalImages;

public class AddAnimalImagesEndpoint(IAnimalImageUploader imageUploader)
    : Endpoint<AddAnimalImagesRequest>
{
    public override void Configure()
    {
        Post("animals/{id}/images");
        AllowFileUploads(true);
    }

    public override async Task HandleAsync(AddAnimalImagesRequest request, CancellationToken cancellationToken)
    {
        var blobNames = new List<string>();

        await foreach (var section in FormFileSectionsAsync(cancellationToken))
        {
            if (section is null) continue;

            var blobName = await imageUploader.UploadImageAsync(request.Id, section.FileName, section.Section.Body,
                cancellationToken);
            blobNames.Add(blobName);
        }

        await SendOkAsync(blobNames, cancellationToken);
    }
}