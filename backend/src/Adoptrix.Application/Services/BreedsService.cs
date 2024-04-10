using Adoptrix.Application.Contracts.Requests.Breeds;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;

namespace Adoptrix.Application.Services;

public interface IBreedsService
{
    Task<IEnumerable<SearchBreedsResult>> SearchAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken = default);


}

public sealed class BreedsService(IBreedsRepository breedsRepository)
    : IBreedsService
{
    public Task<IEnumerable<SearchBreedsResult>> SearchAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken)
    {
        return breedsRepository.SearchAsync(request.SpeciesId, request.WithAnimals, cancellationToken);
    }
}
