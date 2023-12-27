using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FluentResults;

namespace Adoptrix.Infrastructure.Storage.Services;

public abstract class BlobContainerManager(BlobContainerClient containerClient)
{
    protected async Task UploadBlobAsync(string blobName, Stream stream, string contentType,
        CancellationToken cancellationToken)
    {
        var blobClient = containerClient.GetBlobClient(blobName);
        var options = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
        };

        await blobClient.UploadAsync(stream, options, cancellationToken);
    }

    protected async Task<Result> DeleteBlobAsync(string blobName, CancellationToken cancellationToken)
    {
        var blobClient = containerClient.GetBlobClient(blobName);
        var response = await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots,
            cancellationToken: cancellationToken);

        return Result.OkIf(response.Value, $"Blob {blobName} was not found.");
    }

    protected async Task<Stream> OpenReadStreamAsync(string blobName, CancellationToken cancellationToken)
    {
        var blobClient = containerClient.GetBlobClient(blobName);
        return await blobClient.OpenReadAsync(cancellationToken: cancellationToken);
    }
}