using Adoptrix.Application.Models;
using Adoptrix.Domain.Models;
using FluentResults;

namespace Adoptrix.Application.Services;

public interface IAnimalsRepository
{
    Task<IEnumerable<SearchAnimalsResult>> SearchAsync(string? animalName = null, Guid? breedId = null,
        CancellationToken cancellationToken = default);

    Task<Result<Animal>> GetAsync(Guid animalId, CancellationToken cancellationToken = default);

    Task<Result<Animal>> AddAsync(Animal animal, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(Animal animal, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid animalId, CancellationToken cancellationToken = default);
}
