using Adoptrix.Domain;
using FluentResults;

namespace Adoptrix.Application.Services;

public interface IAnimalImageManager
{
    Uri GetImageUri(Guid animalId, Guid imageId, ImageCategory category);

    Task UploadImageAsync(Guid animalId, ImageInformation information, Stream imageStream,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteImageAsync(Guid animalId, Guid imageId, CancellationToken cancellationToken = default);
}