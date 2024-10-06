using Adoptrix.Core.Events;
using FluentResults;

namespace Adoptrix.Logic.Abstractions;

public interface IAnimalImagesManager
{
    Task<Result> ProcessOriginalAsync(AnimalImageAddedEvent data, CancellationToken cancellationToken = default);
    Task DeleteImagesAsync(AnimalDeletedEvent data, CancellationToken cancellationToken = default);
}
