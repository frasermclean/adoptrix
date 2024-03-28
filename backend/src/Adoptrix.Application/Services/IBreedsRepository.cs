using Adoptrix.Application.Models;
using Adoptrix.Domain;
using FluentResults;

namespace Adoptrix.Application.Services;

public interface IBreedsRepository
{
    Task<IEnumerable<SearchBreedsResult>> SearchAsync(Guid? speciesId = null, bool? withAnimals = null,
        CancellationToken cancellationToken = default);

    Task<Result<Breed>> GetByIdAsync(Guid breedId, CancellationToken cancellationToken = default);
    Task<Result<Breed>> GetByNameAsync(string breedName, CancellationToken cancellationToken = default);
    Task<Result> AddAsync(Breed breed, CancellationToken cancellationToken = default);
    Task<Result<Breed>> UpdateAsync(Breed breed, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid breedId, CancellationToken cancellationToken = default);
}
