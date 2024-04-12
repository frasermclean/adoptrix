using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FluentResults;

namespace Adoptrix.Storage.Services;

public abstract class BlobContainerManager(BlobContainerClient containerClient)
{
    protected readonly BlobContainerClient ContainerClient = containerClient;

    protected async Task<Result> UploadBlobAsync(string blobName, Stream stream, string contentType,
        CancellationToken cancellationToken)
    {
        var blobClient = ContainerClient.GetBlobClient(blobName);
        var options = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
        };

        var response = await blobClient.UploadAsync(stream, options, cancellationToken);
        return Result.FailIf(response.GetRawResponse().IsError, $"Blob {blobName} was not created.");
    }

    protected async Task<Result> DeleteBlobAsync(string blobName, CancellationToken cancellationToken)
    {
        var blobClient = ContainerClient.GetBlobClient(blobName);
        var response = await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots,
            cancellationToken: cancellationToken);

        return Result.OkIf(response.Value, $"Blob {blobName} was not found.");
    }

    protected async Task<Stream> OpenReadStreamAsync(string blobName, CancellationToken cancellationToken)
    {
        var blobClient = ContainerClient.GetBlobClient(blobName);
        return await blobClient.OpenReadAsync(cancellationToken: cancellationToken);
    }
}
