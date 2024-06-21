using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Species;
using MediatR;

namespace Adoptrix.Client.Handlers.Species;

public class SearchSpeciesQueryHandler : IRequestHandler<SearchSpeciesQuery, IEnumerable<SpeciesMatch>>
{
    public Task<IEnumerable<SpeciesMatch>> Handle(SearchSpeciesQuery request, CancellationToken cancellationToken)
    {
        var matches = Enumerable.Empty<SpeciesMatch>();
        return Task.FromResult(matches);
    }
}
