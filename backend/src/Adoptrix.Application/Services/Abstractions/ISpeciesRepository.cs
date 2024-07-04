using Adoptrix.Domain.Contracts.Requests.Species;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Models;

namespace Adoptrix.Application.Services.Abstractions;

public interface ISpeciesRepository
{
    Task<IEnumerable<SpeciesMatch>> SearchAsync(SearchSpeciesRequest request, CancellationToken cancellationToken = default);
    Task<Species?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Species?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
