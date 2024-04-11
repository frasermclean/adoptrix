using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services;
using MediatR;

namespace Adoptrix.Application.Handlers.Animals;

public class SearchAnimalsHandler(IAnimalsRepository animalsRepository)
    : IRequestHandler<SearchAnimalsRequest, IEnumerable<SearchAnimalsResult>>
{
    public Task<IEnumerable<SearchAnimalsResult>> Handle(SearchAnimalsRequest request,
        CancellationToken cancellationToken)
    {
        return animalsRepository.SearchAsync(request, cancellationToken);
    }
}
