using Adoptrix.Core.Contracts.Requests.Breeds;
using Adoptrix.Core.Contracts.Responses;

namespace Adoptrix.Core.Abstractions;

public interface IBreedsRepository
{
    Task<IEnumerable<BreedMatch>> SearchAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken = default);

    Task<Breed?> GetByIdAsync(Guid breedId, CancellationToken cancellationToken = default);
    Task<Breed?> GetByNameAsync(string breedName, CancellationToken cancellationToken = default);
    Task AddAsync(Breed breed, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task DeleteAsync(Breed breed, CancellationToken cancellationToken = default);
}
