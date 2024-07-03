using Adoptrix.Domain.Commands.Animals;
using Adoptrix.Domain.Contracts.Requests.Animals;
using Adoptrix.Domain.Contracts.Responses;
using FluentResults;

namespace Adoptrix.Domain.Services;

public interface IAnimalsService
{
    Task<IEnumerable<AnimalMatch>> SearchAsync(SearchAnimalsRequest request, CancellationToken cancellationToken = default);
    Task<Result<AnimalResponse>> GetAsync(Guid animalId, CancellationToken cancellationToken = default);
    Task<Result<AnimalResponse>> AddAsync(AddAnimalRequest request, CancellationToken cancellationToken = default);

    Task<Result<AnimalResponse>> UpdateAsync(UpdateAnimalRequest request,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(Guid animalId, CancellationToken cancellationToken = default);

    Task<Result<AnimalResponse>> AddImagesAsync(AddAnimalImagesCommand command,
        CancellationToken cancellationToken = default);
}
