using Adoptrix.Domain.Models;
using FluentResults;

namespace Adoptrix.Application.Services;

public interface IAnimalImageManager
{
    Uri ContainerUri { get; }

    Task<Result> UploadImageAsync(Guid animalId, Guid imageId, Stream imageStream, string contentType,
        AnimalImageCategory category = AnimalImageCategory.Original, CancellationToken cancellationToken = default);

    Task<Result<int>> DeleteAnimalImagesAsync(Guid animalId, CancellationToken cancellationToken = default);

    Task<Stream> GetImageReadStreamAsync(Guid animalId, Guid imageId, AnimalImageCategory category = AnimalImageCategory.Original,
        CancellationToken cancellationToken = default);
}
