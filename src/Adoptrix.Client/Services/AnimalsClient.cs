using Adoptrix.Client.Extensions;
using Adoptrix.Core.Contracts.Requests.Animals;
using Adoptrix.Core.Contracts.Responses;

namespace Adoptrix.Client.Services;

public interface IAnimalsClient
{
    Task<IEnumerable<AnimalMatch>> SearchAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken = default);

    Task<AnimalResponse> GetAsync(Guid animalId, CancellationToken cancellationToken = default);
}

public class AnimalsClient(HttpClient httpClient) : IAnimalsClient
{
    public async Task<IEnumerable<AnimalMatch>> SearchAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken = default)
    {
        var message = await httpClient.GetAsync($"api/animals{request.ToQueryString()}", cancellationToken);
        message.EnsureSuccessStatusCode();

        var matches = await message.Content.ReadFromJsonAsync<IEnumerable<AnimalMatch>>(cancellationToken);

        return matches ?? [];
    }

    public async Task<AnimalResponse> GetAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var message = await httpClient.GetAsync($"api/animals/{animalId}", cancellationToken);
        message.EnsureSuccessStatusCode();

        var response = await message.Content.ReadFromJsonAsync<AnimalResponse>(cancellationToken);

        return response!;
    }
}
