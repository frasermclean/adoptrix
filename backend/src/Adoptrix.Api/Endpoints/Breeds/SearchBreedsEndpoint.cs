using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Adoptrix.Api.Endpoints.Breeds;

public class SearchBreedsEndpoint
{
    public static async Task<IEnumerable<BreedResponse>> ExecuteAsync(
        IBreedsRepository breedsRepository,
        ISpeciesRepository speciesRepository,
        bool withAnimals = false,
        [FromQuery(Name = "species")] string? speciesIdOrName = null,
        CancellationToken cancellationToken = default)
    {
        var speciesResult = speciesIdOrName is not null
            ? Guid.TryParse(speciesIdOrName, out var speciesId)
                ? await speciesRepository.GetByIdAsync(speciesId, cancellationToken)
                : await speciesRepository.GetByNameAsync(speciesIdOrName, cancellationToken)
            : null;

        var results = await breedsRepository.SearchAsync(speciesResult?.ValueOrDefault, withAnimals, cancellationToken);
        return results.Select(result => result.ToResponse());
    }
}
