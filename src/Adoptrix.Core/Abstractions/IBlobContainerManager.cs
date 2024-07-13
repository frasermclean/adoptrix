namespace Adoptrix.Core.Abstractions;

public interface IBlobContainerManager
{
    string ContainerName { get; }
    Uri ContainerUri { get; }

    Task UploadBlobAsync(string blobName, Stream stream, string contentType,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<string>> GetBlobNamesAsync(string prefix, CancellationToken cancellationToken = default);

    Task DeleteBlobAsync(string blobName, CancellationToken cancellationToken = default);

    Task<Stream> OpenReadStreamAsync(string blobName, CancellationToken cancellationToken = default);
}
