using Adoptrix.Domain.Contracts.Requests.Breeds;
using Adoptrix.Domain.Contracts.Responses;
using FluentResults;

namespace Adoptrix.Domain.Services;

public interface IBreedsService
{
    Task<IEnumerable<BreedMatch>> SearchAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<BreedResponse>> GetAsync(Guid breedId, CancellationToken cancellationToken = default);
    Task<Result<BreedResponse>> AddAsync(AddBreedRequest request, CancellationToken cancellationToken = default);
    Task<Result<BreedResponse>> UpdateAsync(UpdateBreedRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Guid breedId, CancellationToken cancellationToken = default);
}
