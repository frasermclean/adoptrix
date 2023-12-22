using Adoptrix.Domain;
using FluentResults;

namespace Adoptrix.Application.Services;

public interface IAnimalImageManager
{
    Task UploadImageAsync(Guid animalId, Guid imageId, Stream imageStream, string contentType,
        ImageCategory category = ImageCategory.Original, CancellationToken cancellationToken = default);

    Task<Result> DeleteImageAsync(Guid animalId, Guid imageId, ImageCategory category = ImageCategory.Original,
        CancellationToken cancellationToken = default);

    Task<Stream> GetImageReadStreamAsync(Guid animalId, Guid imageId,
        ImageCategory category,
        CancellationToken cancellationToken = default);
}