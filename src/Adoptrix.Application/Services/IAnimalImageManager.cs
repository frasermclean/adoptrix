using FluentResults;

namespace Adoptrix.Application.Services;

public interface IAnimalImageManager
{
    string GenerateFileName(Guid animalId, string contentType, string originalFileName);
    Task<string> UploadImageAsync(string blobName, Stream imageStream, string contentType,
        CancellationToken cancellationToken = default);
    Task<Result> DeleteImageAsync(Guid animalId, string fileName, CancellationToken cancellationToken = default);
}