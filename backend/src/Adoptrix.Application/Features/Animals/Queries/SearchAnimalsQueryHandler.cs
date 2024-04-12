using Adoptrix.Application.Features.Animals.Responses;
using Adoptrix.Application.Services;
using MediatR;

namespace Adoptrix.Application.Features.Animals.Queries;

public class SearchAnimalsQueryHandler(IAnimalsRepository animalsRepository)
    : IRequestHandler<SearchAnimalsQuery, IEnumerable<SearchAnimalsResult>>
{
    public Task<IEnumerable<SearchAnimalsResult>> Handle(SearchAnimalsQuery query,
        CancellationToken cancellationToken)
    {
        return animalsRepository.SearchAsync(query, cancellationToken);
    }
}
