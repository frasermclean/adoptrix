using Adoptrix.Application.Contracts.Requests.Species;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Services.Repositories;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Handlers.Species;

public class GetSpeciesHandler(ISpeciesRepository speciesRepository)
    : IRequestHandler<GetSpeciesRequest, Result<Domain.Models.Species>>
{
    public async Task<Result<Domain.Models.Species>> Handle(GetSpeciesRequest request,
        CancellationToken cancellationToken)
    {
        var species = Guid.TryParse(request.SpeciesIdOrName, out var speciesId)
            ? await speciesRepository.GetByIdAsync(speciesId, cancellationToken)
            : await speciesRepository.GetByNameAsync(request.SpeciesIdOrName, cancellationToken);

        if (species is null)
        {
            return speciesId != Guid.Empty
                ? new SpeciesNotFoundError(speciesId)
                : new SpeciesNotFoundError(request.SpeciesIdOrName);
        }

        return species;
    }
}
