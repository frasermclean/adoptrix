using Adoptrix.Application.Features.Species.Queries;
using Adoptrix.Application.Features.Species.Responses;
using Adoptrix.Domain.Models;

namespace Adoptrix.Application.Services;

public interface ISpeciesRepository
{
    Task<IEnumerable<SearchSpeciesMatch>> SearchAsync(SearchSpeciesQuery query, CancellationToken cancellationToken = default);
    Task<Species?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Species?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
