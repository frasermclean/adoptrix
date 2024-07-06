using Adoptrix.Application.Mapping;
using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Core.Contracts.Requests.Species;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Core.Errors;
using Adoptrix.Core.Services;
using FluentResults;

namespace Adoptrix.Application.Services;

public class SpeciesService(ISpeciesRepository speciesRepository) : ISpeciesService
{
    public Task<IEnumerable<SpeciesMatch>> SearchAsync(SearchSpeciesRequest request, CancellationToken cancellationToken = default)
    {
        return speciesRepository.SearchAsync(request, cancellationToken);
    }

    public async Task<Result<SpeciesResponse>> GetAsync(Guid speciesId, CancellationToken cancellationToken = default)
    {
        var species = await speciesRepository.GetByIdAsync(speciesId, cancellationToken);

        return species is null
            ? new SpeciesNotFoundError(speciesId)
            : species.ToResponse();
    }
}
