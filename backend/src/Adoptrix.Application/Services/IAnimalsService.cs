using Adoptrix.Application.Contracts.Requests;
using Adoptrix.Application.Models;
using Adoptrix.Domain.Models;
using FluentResults;

namespace Adoptrix.Application.Services;

public interface IAnimalsService
{
    Task<IEnumerable<SearchAnimalsResult>> SearchAsync(string? animalName = null, Guid? breedId = null,
        CancellationToken cancellationToken = default);

    Task<Result<Animal>> GetAsync(Guid animalId, CancellationToken cancellationToken = default);
    Task<Result<Animal>> AddAsync(SetAnimalRequest request, CancellationToken cancellationToken = default);
    Task<Result<Animal>> UpdateAsync(Guid animalId, SetAnimalRequest request,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(Guid animalId, CancellationToken cancellationToken = default);

    Task<Result<Animal>> AddImagesAsync(Guid animalId, IEnumerable<AnimalImage> images,
        CancellationToken cancellationToken = default);

    Task<Result> SetImageProcessedAsync(Guid animalId, Guid imageId, CancellationToken cancellationToken = default);
}
