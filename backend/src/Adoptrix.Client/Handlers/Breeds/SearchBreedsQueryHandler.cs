using System.Net.Http.Json;
using Adoptrix.Client.Extensions;
using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Breeds;
using MediatR;

namespace Adoptrix.Client.Handlers.Breeds;

public class SearchBreedsQueryHandler(HttpClient httpClient)
    : IRequestHandler<SearchBreedsQuery, IEnumerable<BreedMatch>>
{
    public async Task<IEnumerable<BreedMatch>> Handle(SearchBreedsQuery query, CancellationToken cancellationToken)
    {
        var queryString = query.ToQueryString();
        var message = await httpClient.GetAsync($"api/breeds{queryString}", cancellationToken);
        var matches = await message.Content.ReadFromJsonAsync<IEnumerable<BreedMatch>>(cancellationToken);

        return matches ?? [];
    }
}
