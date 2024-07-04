using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Models;
using FluentResults;

namespace Adoptrix.Application.Services;

public interface IAnimalImageManager
{
    Uri ContainerUri { get; }

    Task<Result<AnimalImage>> UploadOriginalAsync(Guid animalId, Guid userId, string fileName, string description,
        string contentType, Stream stream, CancellationToken cancellationToken = default);

    Task<Result<AnimalResponse>> AddImagesToAnimalAsync(Animal animal, IReadOnlyCollection<AnimalImage> images,
        CancellationToken cancellationToken = default);

    Task<Result> UploadImageAsync(Guid animalId, Guid imageId, Stream imageStream, string contentType,
        AnimalImageCategory category = AnimalImageCategory.Original, CancellationToken cancellationToken = default);

    Task<Result<int>> DeleteAnimalImagesAsync(Guid animalId, CancellationToken cancellationToken = default);

    Task<Stream> GetImageReadStreamAsync(Guid animalId, Guid imageId,
        AnimalImageCategory category = AnimalImageCategory.Original,
        CancellationToken cancellationToken = default);
}
