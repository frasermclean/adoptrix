using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Breeds;
using MediatR;

namespace Adoptrix.Client.Handlers.Breeds;

public class SearchBreedsQueryHandler : IRequestHandler<SearchBreedsQuery, IEnumerable<BreedMatch>>
{
    public Task<IEnumerable<BreedMatch>> Handle(SearchBreedsQuery request, CancellationToken cancellationToken)
    {
        var matches = Enumerable.Empty<BreedMatch>();
        return Task.FromResult(matches);
    }
}
