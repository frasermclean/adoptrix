using Adoptrix.Core.Contracts.Requests.Animals;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Web.Extensions;

namespace Adoptrix.Web.Services;

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

        var matches = await message.DeserializeJsonContentAsync<IEnumerable<AnimalMatch>>(cancellationToken);

        return matches ?? [];
    }

    public async Task<AnimalResponse> GetAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var message = await httpClient.GetAsync($"api/animals/{animalId}", cancellationToken);
        message.EnsureSuccessStatusCode();

        var response = await message.DeserializeJsonContentAsync<AnimalResponse>(cancellationToken);

        return response!;
    }
}
