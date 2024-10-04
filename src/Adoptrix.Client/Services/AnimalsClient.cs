using Adoptrix.Client.Extensions;
using Adoptrix.Core.Requests;
using Adoptrix.Core.Responses;

namespace Adoptrix.Client.Services;

public interface IAnimalsClient
{
    Task<IEnumerable<AnimalMatch>> SearchAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken = default);

    Task<AnimalResponse> GetAsync(string animalSlug, CancellationToken cancellationToken = default);
}

public class AnimalsClient(HttpClient httpClient) : IAnimalsClient
{
    public async Task<IEnumerable<AnimalMatch>> SearchAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken = default)
    {
        var message = await httpClient.GetAsync($"api/animals{request.ToQueryString()}", cancellationToken);
        message.EnsureSuccessStatusCode();

        var matches = await message.DeserializeJsonContentAsync<IEnumerable<AnimalMatch>>(cancellationToken);

        return matches ?? [];
    }

    public async Task<AnimalResponse> GetAsync(string animalSlug, CancellationToken cancellationToken = default)
    {
        var message = await httpClient.GetAsync($"api/animals/{animalSlug}", cancellationToken);
        message.EnsureSuccessStatusCode();

        var response = await message.DeserializeJsonContentAsync<AnimalResponse>(cancellationToken);

        return response!;
    }
}
