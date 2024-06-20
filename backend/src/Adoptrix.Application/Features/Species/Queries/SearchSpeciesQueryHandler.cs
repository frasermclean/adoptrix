using Adoptrix.Application.Services;
using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Species;
using MediatR;


namespace Adoptrix.Application.Features.Species.Queries;

public class SearchSpeciesQueryHandler(ISpeciesRepository speciesRepository)
    : IRequestHandler<SearchSpeciesQuery, IEnumerable<SpeciesMatch>>
{
    public Task<IEnumerable<SpeciesMatch>> Handle(SearchSpeciesQuery query, CancellationToken cancellationToken)
    {
        return speciesRepository.SearchAsync(query, cancellationToken);
    }
}
