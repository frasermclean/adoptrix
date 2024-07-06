using Adoptrix.Core.Contracts.Requests.Breeds;
using Adoptrix.Core.Contracts.Responses;
using FluentResults;

namespace Adoptrix.Core.Services;

public interface IBreedsService
{
    Task<IEnumerable<BreedMatch>> SearchAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<BreedResponse>> GetAsync(Guid breedId, CancellationToken cancellationToken = default);
    Task<Result<BreedResponse>> AddAsync(AddBreedRequest request, CancellationToken cancellationToken = default);
    Task<Result<BreedResponse>> UpdateAsync(UpdateBreedRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid breedId, CancellationToken cancellationToken = default);
}
