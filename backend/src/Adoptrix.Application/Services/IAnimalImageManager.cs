using Adoptrix.Domain;
using FluentResults;

namespace Adoptrix.Application.Services;

public interface IAnimalImageManager
{
    Task<Result> UploadImageAsync(Guid animalId, Guid imageId, Stream imageStream, string contentType,
        ImageCategory category = ImageCategory.Original, CancellationToken cancellationToken = default);

    Task DeleteAnimalImagesAsync(Guid animalId, CancellationToken cancellationToken = default);

    Task<Stream> GetImageReadStreamAsync(Guid animalId, Guid imageId,
        ImageCategory category,
        CancellationToken cancellationToken = default);
}