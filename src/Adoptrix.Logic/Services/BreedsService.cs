using Adoptrix.Core.Requests;
using Adoptrix.Core.Responses;
using Adoptrix.Logic.Abstractions;
using Adoptrix.Logic.Errors;
using Adoptrix.Logic.Mapping;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Logic.Services;

public interface IBreedsService
{
    Task<Result<BreedResponse>> UpdateAsync(UpdateBreedRequest request, CancellationToken cancellationToken = default);
}

public class BreedsService(
    ILogger<BreedsService> logger,
    IBreedsRepository breedsRepository,
    ISpeciesRepository speciesRepository) : IBreedsService
{
    public async Task<Result<BreedResponse>> UpdateAsync(UpdateBreedRequest request,
        CancellationToken cancellationToken)
    {
        // ensure breed exists
        var breed = await breedsRepository.GetAsync(request.BreedId, cancellationToken);
        if (breed is null)
        {
            return new BreedNotFoundError(request.BreedId);
        }

        // ensure species exists
        var species = await speciesRepository.GetAsync(request.SpeciesName, cancellationToken);
        if (species is null)
        {
            return new SpeciesNotFoundError(request.SpeciesName);
        }

        breed.Update(request, species);
        var result = await breedsRepository.UpdateAsync(breed, cancellationToken);

        if (result.IsSuccess)
        {
            logger.LogInformation("Updated breed with ID {BreedId}", request.BreedId);
            return breed.ToResponse();
        }

        if (result.HasError<DuplicateBreedError>())
        {
            logger.LogError("Breed with name {BreedName} already exists", request.Name);
        }

        return result;
    }
}
