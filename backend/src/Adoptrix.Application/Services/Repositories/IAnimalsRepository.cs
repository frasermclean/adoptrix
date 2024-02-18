using Adoptrix.Application.Models;
using Adoptrix.Domain;
using FluentResults;

namespace Adoptrix.Application.Services.Repositories;

public interface IAnimalsRepository
{
    Task<IEnumerable<SearchAnimalsResult>> SearchAnimalsAsync(string? animalName = null, string? speciesName = null,
        CancellationToken cancellationToken = default);

    Task<Result<Animal>> GetAsync(Guid animalId, CancellationToken cancellationToken = default);

    Task<Result<Animal>> AddAsync(Animal animal, CancellationToken cancellationToken = default);
    Task<Result<Animal>> UpdateAsync(Animal animal, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid animalId, CancellationToken cancellationToken = default);
}
