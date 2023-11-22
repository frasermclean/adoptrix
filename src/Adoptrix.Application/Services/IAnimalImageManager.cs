using Adoptrix.Domain;

namespace Adoptrix.Application.Services;

public interface IAnimalImageManager
{
    string GenerateFileName(string contentType, string originalFileName);
    Task<string> UploadImageAsync(string blobName, Stream imageStream, string contentType,
        CancellationToken cancellationToken = default);
}