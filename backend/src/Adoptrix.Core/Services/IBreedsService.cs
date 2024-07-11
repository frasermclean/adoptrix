using Adoptrix.Core.Contracts.Requests.Breeds;
using Adoptrix.Core.Contracts.Responses;
using FluentResults;

namespace Adoptrix.Core.Services;

public interface IBreedsService
{
    Task<Result<BreedResponse>> UpdateAsync(UpdateBreedRequest request, CancellationToken cancellationToken = default);
}
