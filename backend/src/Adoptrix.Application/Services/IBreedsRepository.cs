using Adoptrix.Application.Features.Breeds.Responses;
using Adoptrix.Domain.Models;

namespace Adoptrix.Application.Services;

public interface IBreedsRepository
{
    Task<IEnumerable<SearchBreedsResult>> SearchAsync(Guid? speciesId = null, bool? withAnimals = null,
        CancellationToken cancellationToken = default);

    Task<Breed?> GetByIdAsync(Guid breedId, CancellationToken cancellationToken = default);
    Task<Breed?> GetByNameAsync(string breedName, CancellationToken cancellationToken = default);
    Task AddAsync(Breed breed, CancellationToken cancellationToken = default);
    Task UpdateAsync(Breed breed, CancellationToken cancellationToken = default);
    Task DeleteAsync(Breed breed, CancellationToken cancellationToken = default);
}
