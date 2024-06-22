using System.Net.Http.Json;
using Adoptrix.Client.Extensions;
using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Species;
using MediatR;

namespace Adoptrix.Client.Handlers.Species;

public class SearchSpeciesQueryHandler(HttpClient httpClient)
    : IRequestHandler<SearchSpeciesQuery, IEnumerable<SpeciesMatch>>
{
    public async Task<IEnumerable<SpeciesMatch>> Handle(SearchSpeciesQuery query, CancellationToken cancellationToken)
    {
        var queryString = query.ToQueryString();
        var message = await httpClient.GetAsync($"api/species{queryString}", cancellationToken);
        var matches = await message.Content.ReadFromJsonAsync<IEnumerable<SpeciesMatch>>(cancellationToken);

        return matches ?? [];
    }
}
