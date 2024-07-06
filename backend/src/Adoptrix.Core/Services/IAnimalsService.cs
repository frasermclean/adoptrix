using Adoptrix.Core.Contracts.Requests.Animals;
using Adoptrix.Core.Contracts.Responses;
using FluentResults;

namespace Adoptrix.Core.Services;

public interface IAnimalsService
{
    Task<IEnumerable<AnimalMatch>> SearchAsync(SearchAnimalsRequest request, CancellationToken cancellationToken = default);
    Task<Result<AnimalResponse>> GetAsync(Guid animalId, CancellationToken cancellationToken = default);
    Task<Result<AnimalResponse>> AddAsync(AddAnimalRequest request, CancellationToken cancellationToken = default);

    Task<Result<AnimalResponse>> UpdateAsync(UpdateAnimalRequest request,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(Guid animalId, CancellationToken cancellationToken = default);
}
