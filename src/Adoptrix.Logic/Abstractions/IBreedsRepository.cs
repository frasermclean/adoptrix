using Adoptrix.Core;
using FluentResults;

namespace Adoptrix.Logic.Abstractions;

public interface IBreedsRepository
{
    Task<Breed?> GetAsync(int breedId, CancellationToken cancellationToken = default);
    Task<Result> AddAsync(Breed breed, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(Breed breed, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Breed breed, CancellationToken cancellationToken = default);
}
