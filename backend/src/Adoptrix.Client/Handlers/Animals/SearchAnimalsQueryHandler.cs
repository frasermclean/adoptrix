using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Animals;
using MediatR;

namespace Adoptrix.Client.Handlers.Animals;

public class SearchAnimalsQueryHandler : IRequestHandler<SearchAnimalsQuery, IEnumerable<AnimalMatch>>
{
    public Task<IEnumerable<AnimalMatch>> Handle(SearchAnimalsQuery query,
        CancellationToken cancellationToken)
    {
        var matches = Enumerable.Empty<AnimalMatch>();
        return Task.FromResult(matches);
    }
}
