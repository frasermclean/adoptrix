using FluentResults;

namespace Adoptrix.Application.Services;

public interface IAnimalImageManager
{
    string GenerateFileName(int animalId, string contentType, string originalFileName);
    Uri GetImageUri(int animalId, string fileName);

    Task<string> UploadImageAsync(int animalId, string fileName, Stream imageStream, string contentType,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteImageAsync(int animalId, string fileName, CancellationToken cancellationToken = default);
}