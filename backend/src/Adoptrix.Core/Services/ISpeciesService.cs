using Adoptrix.Core.Contracts.Requests.Species;
using Adoptrix.Core.Contracts.Responses;
using FluentResults;

namespace Adoptrix.Core.Services;

public interface ISpeciesService
{
    Task<IEnumerable<SpeciesMatch>> SearchAsync(SearchSpeciesRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<SpeciesResponse>> GetAsync(Guid speciesId, CancellationToken cancellationToken = default);
}
