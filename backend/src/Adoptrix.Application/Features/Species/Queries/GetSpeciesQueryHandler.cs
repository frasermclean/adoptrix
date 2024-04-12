using Adoptrix.Application.Errors;
using Adoptrix.Application.Services;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Species.Queries;

public class GetSpeciesQueryHandler(ISpeciesRepository speciesRepository)
    : IRequestHandler<GetSpeciesQuery, Result<Domain.Models.Species>>
{
    public async Task<Result<Domain.Models.Species>> Handle(GetSpeciesQuery query,
        CancellationToken cancellationToken)
    {
        var species = Guid.TryParse(query.SpeciesIdOrName, out var speciesId)
            ? await speciesRepository.GetByIdAsync(speciesId, cancellationToken)
            : await speciesRepository.GetByNameAsync(query.SpeciesIdOrName, cancellationToken);

        if (species is null)
        {
            return speciesId != Guid.Empty
                ? new SpeciesNotFoundError(speciesId)
                : new SpeciesNotFoundError(query.SpeciesIdOrName);
        }

        return species;
    }
}
