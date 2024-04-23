using Adoptrix.Application.Features.Species.Responses;
using Adoptrix.Application.Services;
using MediatR;


namespace Adoptrix.Application.Features.Species.Queries;

public class SearchSpeciesQueryHandler(ISpeciesRepository speciesRepository)
    : IRequestHandler<SearchSpeciesQuery, IEnumerable<SearchSpeciesMatch>>
{
    public Task<IEnumerable<SearchSpeciesMatch>> Handle(SearchSpeciesQuery query, CancellationToken cancellationToken)
    {
        return speciesRepository.SearchAsync(query, cancellationToken);
    }
}
