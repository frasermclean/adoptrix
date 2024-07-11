using Adoptrix.Core.Contracts.Requests.Animals;
using Adoptrix.Core.Contracts.Responses;
using FluentResults;

namespace Adoptrix.Core.Services;

public interface IAnimalsService
{
    Task<Result<AnimalResponse>> UpdateAsync(UpdateAnimalRequest request,
        CancellationToken cancellationToken = default);
}
