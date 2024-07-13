using Adoptrix.Core.Contracts.Requests.Species;
using Adoptrix.Core.Contracts.Responses;

namespace Adoptrix.Core.Abstractions;

public interface ISpeciesRepository
{
    Task<IEnumerable<SpeciesMatch>> SearchAsync(SearchSpeciesRequest request, CancellationToken cancellationToken = default);
    Task<Species?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Species?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
