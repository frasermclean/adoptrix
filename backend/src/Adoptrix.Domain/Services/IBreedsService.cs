using Adoptrix.Domain.Contracts.Requests;
using Adoptrix.Domain.Contracts.Responses;
using FluentResults;

namespace Adoptrix.Domain.Services;

public interface IBreedsService
{
    Task<IEnumerable<BreedMatch>> SearchAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<BreedResponse>> GetAsync(Guid breedId, CancellationToken cancellationToken = default);
}
