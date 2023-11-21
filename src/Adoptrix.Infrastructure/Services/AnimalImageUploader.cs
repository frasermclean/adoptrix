using Adoptrix.Application.Services;
using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Infrastructure.Services;

public class AnimalImageUploader([FromKeyedServices("animal-images")] BlobContainerClient containerClient)
    : IAnimalImageUploader
{
    public async Task<string> UploadImageAsync(Guid animalId, string fileName, Stream imageStream,
        CancellationToken cancellationToken)
    {
        var blobName = $"{animalId}/{fileName}";
        var blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.UploadAsync(imageStream, true, cancellationToken);

        return blobName;
    }
}