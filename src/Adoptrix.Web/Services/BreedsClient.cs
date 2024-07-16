using Adoptrix.Core.Contracts.Requests.Breeds;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Web.Extensions;

namespace Adoptrix.Web.Services;

public interface IBreedsClient
{
    Task<IEnumerable<BreedMatch>> SearchAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken = default);
}

public class BreedsClient(HttpClient httpClient) : IBreedsClient
{
    public async Task<IEnumerable<BreedMatch>> SearchAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken = default)
    {
        var message = await httpClient.GetAsync($"api/breeds{request.ToQueryString()}", cancellationToken);
        message.EnsureSuccessStatusCode();

        var matches = await message.DeserializeJsonContentAsync<IEnumerable<BreedMatch>>(cancellationToken);

        return matches ?? [];
    }
}
