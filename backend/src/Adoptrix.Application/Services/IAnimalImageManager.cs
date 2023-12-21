using Adoptrix.Domain;
using FluentResults;

namespace Adoptrix.Application.Services;

public interface IAnimalImageManager
{
    Task UploadImageAsync(Guid animalId, ImageInformation information, Stream imageStream,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteImageAsync(Guid animalId, Guid imageId, CancellationToken cancellationToken = default);
    Task<Stream> GetOriginalImageAsync(Guid animalId, Guid imageId, CancellationToken cancellationToken = default);
}