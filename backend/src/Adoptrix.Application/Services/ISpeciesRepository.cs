using Adoptrix.Domain.Models;
using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Species;

namespace Adoptrix.Application.Services;

public interface ISpeciesRepository
{
    Task<IEnumerable<SpeciesMatch>> SearchAsync(SearchSpeciesQuery query, CancellationToken cancellationToken = default);
    Task<Species?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Species?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
