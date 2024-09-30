using Adoptrix.Client.Extensions;
using Adoptrix.Contracts.Requests;
using Adoptrix.Core.Responses;

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

        var matches = await message.DeserializeJsonContentAsync<IEnumerable<SpeciesMatch>>(cancellationToken);

        return matches ?? [];
    }
}
