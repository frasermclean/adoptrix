using Adoptrix.Application.Mapping;
using Adoptrix.Domain.Contracts.Requests;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Errors;
using Adoptrix.Domain.Services;
using FluentResults;

namespace Adoptrix.Application.Services;

public class BreedsService(IBreedsRepository breedsRepository) : IBreedsService
{
    public Task<IEnumerable<BreedMatch>> SearchAsync(SearchBreedsRequest request, CancellationToken cancellationToken = default)
    {
        return breedsRepository.SearchAsync(request.SpeciesId, request.WithAnimals, cancellationToken);
    }

    public async Task<Result<BreedResponse>> GetAsync(Guid breedId, CancellationToken cancellationToken = default)
    {
        var breed = await breedsRepository.GetByIdAsync(breedId, cancellationToken);

        return breed is not null
            ? breed.ToResponse()
            : new BreedNotFoundError(breedId);
    }
}
