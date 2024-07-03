using Adoptrix.Domain.Contracts.Requests;
using Adoptrix.Domain.Contracts.Requests.Species;
using Adoptrix.Domain.Contracts.Responses;
using FluentResults;

namespace Adoptrix.Domain.Services;

public interface ISpeciesService
{
    Task<IEnumerable<SpeciesMatch>> SearchAsync(SearchSpeciesRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<SpeciesResponse>> GetAsync(Guid speciesId, CancellationToken cancellationToken = default);
}
