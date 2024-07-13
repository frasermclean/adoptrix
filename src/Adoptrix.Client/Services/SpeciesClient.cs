using Adoptrix.Client.Extensions;
using Adoptrix.Core.Contracts.Requests.Species;
using Adoptrix.Core.Contracts.Responses;

namespace Adoptrix.Client.Services;

public interface ISpeciesClient
{
    Task<IEnumerable<SpeciesMatch>> SearchAsync(SearchSpeciesRequest request,
        CancellationToken cancellationToken = default);
}

public class SpeciesClient(HttpClient httpClient) : ISpeciesClient
{
    public async Task<IEnumerable<SpeciesMatch>> SearchAsync(SearchSpeciesRequest request,
        CancellationToken cancellationToken = default)
    {
        var message = await httpClient.GetAsync($"api/species{request.ToQueryString()}", cancellationToken);
        message.EnsureSuccessStatusCode();

        var matches = await message.Content.ReadFromJsonAsync<IEnumerable<SpeciesMatch>>(cancellationToken);

        return matches ?? [];
    }
}
