using System.Net.Http.Json;
using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Animals;
using MediatR;

namespace Adoptrix.Client.Handlers.Animals;

public class SearchAnimalsQueryHandler(HttpClient httpClient)
    : IRequestHandler<SearchAnimalsQuery, IEnumerable<AnimalMatch>>
{
    public async Task<IEnumerable<AnimalMatch>> Handle(SearchAnimalsQuery query,
        CancellationToken cancellationToken)
    {
        var message = await httpClient.GetAsync("api/animals", cancellationToken);
        var matches = await message.Content.ReadFromJsonAsync<IEnumerable<AnimalMatch>>(cancellationToken: cancellationToken);

        return matches ?? [];
    }
}
