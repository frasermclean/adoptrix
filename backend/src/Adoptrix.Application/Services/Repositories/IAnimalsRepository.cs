using Adoptrix.Application.Models;
using Adoptrix.Domain;
using FluentResults;

namespace Adoptrix.Application.Services.Repositories;

public interface IAnimalsRepository
{
    Task<IEnumerable<SearchAnimalsResult>> SearchAsync(string? animalName = null, Guid? speciesId = null,
        CancellationToken cancellationToken = default);

    Task<Result<Animal>> GetAsync(Guid animalId, CancellationToken cancellationToken = default);

    Task<Result<Animal>> AddAsync(Animal animal, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(Animal animal, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid animalId, CancellationToken cancellationToken = default);
}
