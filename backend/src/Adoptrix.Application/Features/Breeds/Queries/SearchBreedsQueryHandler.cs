using Adoptrix.Application.Services;
using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Breeds;
using MediatR;

namespace Adoptrix.Application.Features.Breeds.Queries;

public class SearchBreedsQueryHandler(IBreedsRepository breedsRepository)
    : IRequestHandler<SearchBreedsQuery, IEnumerable<BreedMatch>>
{
    public Task<IEnumerable<BreedMatch>> Handle(SearchBreedsQuery query,
        CancellationToken cancellationToken)
    {
        return breedsRepository.SearchAsync(query.SpeciesId, query.WithAnimals, cancellationToken);
    }
}
