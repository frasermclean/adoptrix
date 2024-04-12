using Adoptrix.Application.Features.Breeds.Responses;
using Adoptrix.Application.Services;
using MediatR;

namespace Adoptrix.Application.Features.Breeds.Queries;

public class SearchBreedsQueryHandler(IBreedsRepository breedsRepository)
    : IRequestHandler<SearchBreedsQuery, IEnumerable<SearchBreedsResult>>
{
    public Task<IEnumerable<SearchBreedsResult>> Handle(SearchBreedsQuery query,
        CancellationToken cancellationToken)
    {
        return breedsRepository.SearchAsync(query.SpeciesId, query.WithAnimals, cancellationToken);
    }
}
