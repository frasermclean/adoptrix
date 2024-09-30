using Adoptrix.Core.Events;
using Adoptrix.Core.Responses;
using Adoptrix.Logic.Models;
using FluentResults;

namespace Adoptrix.Logic.Abstractions;

public interface IAnimalImagesManager
{
    Task<Result<AnimalResponse>> AddOriginalsAsync(Guid animalId, Guid userId,
        IAsyncEnumerable<AddOriginalImageData> items, CancellationToken cancellationToken = default);

    Task<Result> ProcessOriginalAsync(AnimalImageAddedEvent data, CancellationToken cancellationToken = default);
    Task DeleteImagesAsync(AnimalDeletedEvent data, CancellationToken cancellationToken = default);
}
