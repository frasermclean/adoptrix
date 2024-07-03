using Adoptrix.Domain.Commands.Animals;
using Adoptrix.Domain.Contracts.Requests;
using Adoptrix.Domain.Models.Responses;
using FluentResults;

namespace Adoptrix.Domain.Services;

public interface IAnimalsService
{
    Task<IEnumerable<AnimalMatch>> SearchAsync(SearchAnimalsRequest? request = null, CancellationToken cancellationToken = default);
    Task<Result<AnimalResponse>> GetAsync(Guid animalId, CancellationToken cancellationToken = default);
    Task<Result<AnimalResponse>> AddAsync(AddAnimalCommand command, CancellationToken cancellationToken = default);

    Task<Result<AnimalResponse>> UpdateAsync(UpdateAnimalCommand command,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteAsync(Guid animalId, CancellationToken cancellationToken = default);

    Task<Result<AnimalResponse>> AddImagesAsync(AddAnimalImagesCommand command,
        CancellationToken cancellationToken = default);
}
