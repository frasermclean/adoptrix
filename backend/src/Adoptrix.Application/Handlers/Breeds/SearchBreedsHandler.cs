using Adoptrix.Application.Contracts.Requests.Breeds;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using MediatR;

namespace Adoptrix.Application.Handlers.Breeds;

public class SearchBreedsHandler(IBreedsRepository breedsRepository)
    : IRequestHandler<SearchBreedsRequest, IEnumerable<SearchBreedsResult>>
{
    public Task<IEnumerable<SearchBreedsResult>> Handle(SearchBreedsRequest request,
        CancellationToken cancellationToken)
    {
        return breedsRepository.SearchAsync(request.SpeciesId, request.WithAnimals, cancellationToken);
    }
}
