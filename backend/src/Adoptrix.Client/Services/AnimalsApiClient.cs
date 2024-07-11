using System.Net.Http.Json;
using System.Text.Json;
using Adoptrix.Client.Extensions;
using Adoptrix.Core.Contracts.Requests.Animals;
using Adoptrix.Core.Contracts.Responses;

namespace Adoptrix.Client.Services;

public interface IAnimalsApiClient
{
    Task<IEnumerable<AnimalMatch>> SearchAsync(SearchAnimalsRequest request, CancellationToken cancellationToken = default);

    Task<AnimalResponse> GetAsync(Guid animalId, CancellationToken cancellationToken = default);
}

public class AnimalsApiClient(HttpClient httpClient, JsonSerializerOptions serializerOptions) : IAnimalsApiClient
{
    public async Task<IEnumerable<AnimalMatch>> SearchAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken = default)
    {
        var message = await httpClient.GetAsync($"api/animals{request.ToQueryString()}", cancellationToken);
        message.EnsureSuccessStatusCode();

        var matches =
            await message.Content.ReadFromJsonAsync<IEnumerable<AnimalMatch>>(serializerOptions, cancellationToken);

        return matches ?? [];
    }

    public async Task<AnimalResponse> GetAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var message = await httpClient.GetAsync($"api/animals/{animalId}", cancellationToken);
        message.EnsureSuccessStatusCode();

        var response = await message.Content.ReadFromJsonAsync<AnimalResponse>(serializerOptions, cancellationToken);

        return response!;
    }
}
