using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;
using Adoptrix.Web.Extensions;

namespace Adoptrix.Web.Services;

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
