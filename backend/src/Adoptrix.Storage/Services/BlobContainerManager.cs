using Adoptrix.Application.Services.Abstractions;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Adoptrix.Storage.Services;

public class BlobContainerManager(BlobServiceClient blobServiceClient, string containerName) : IBlobContainerManager
{
    private readonly BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

    public string ContainerName => containerClient.Name;
    public Uri ContainerUri => containerClient.Uri;

    public async Task UploadBlobAsync(string blobName, Stream stream, string contentType,
        CancellationToken cancellationToken)
    {
        var blobClient = containerClient.GetBlobClient(blobName);
        var options = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
        };

        await blobClient.UploadAsync(stream, options, cancellationToken);
    }

    public async Task<IEnumerable<string>> GetBlobNamesAsync(string prefix, CancellationToken cancellationToken)
    {
        var pages = containerClient.GetBlobsAsync(prefix: prefix, cancellationToken: cancellationToken)
            .AsPages();

        var blobNames = new List<string>();
        await foreach (var page in pages)
        {
            blobNames.AddRange(page.Values.Select(item => item.Name));
        }

        return blobNames;
    }

    public async Task DeleteBlobAsync(string blobName, CancellationToken cancellationToken)
    {
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots,
            cancellationToken: cancellationToken);
    }

    public async Task<Stream> OpenReadStreamAsync(string blobName, CancellationToken cancellationToken)
    {
        var blobClient = containerClient.GetBlobClient(blobName);
        return await blobClient.OpenReadAsync(cancellationToken: cancellationToken);
    }
}
