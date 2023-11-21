using FluentResults;

namespace Adoptrix.Application.Services;

public interface IAnimalImageUploader
{
    Task<string> UploadImageAsync(Guid animalId, string fileName, Stream imageStream,
        CancellationToken cancellationToken = default);
}