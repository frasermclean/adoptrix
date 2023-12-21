using FluentResults;

namespace Adoptrix.Application.Services;

public interface IAnimalImageManager
{
    string GenerateFileName(Guid animalId, string contentType, string originalFileName);
    Uri GetImageUri(Guid animalId, string fileName);

    Task UploadImageAsync(Guid animalId, string fileName, Stream imageStream, string contentType,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteImageAsync(Guid animalId, string fileName, CancellationToken cancellationToken = default);
}