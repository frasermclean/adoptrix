using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Animals.AddAnimalImages;

public class AddAnimalImagesEndpoint : Endpoint<AddAnimalImagesRequest>
{
    public override void Configure()
    {
        Post("animals/{id}/images");
        AllowFileUploads(true);
    }

    public override async Task HandleAsync(AddAnimalImagesRequest request, CancellationToken cancellationToken)
    {
        const string uploadsDirectoryName = "Uploads";
        Directory.CreateDirectory(uploadsDirectoryName);

        await foreach (var section in FormFileSectionsAsync(cancellationToken))
        {
            if (section is null) continue;

            await using var fileStream = File.Create(Path.Combine(uploadsDirectoryName, section.FileName));
            await section.Section.Body.CopyToAsync(fileStream, 1024 * 64, cancellationToken);
        }

        await SendOkAsync("All done!", cancellationToken);
    }
}