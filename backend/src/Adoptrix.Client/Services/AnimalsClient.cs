using System.Net.Http.Json;
using System.Text.Json;
using Adoptrix.Client.Extensions;
using Adoptrix.Domain.Commands.Animals;
using Adoptrix.Domain.Contracts.Requests.Animals;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Services;
using FluentResults;

namespace Adoptrix.Client.Services;

public class AnimalsClient(HttpClient httpClient, JsonSerializerOptions serializerOptions) : IAnimalsService
{
    public async Task<IEnumerable<AnimalMatch>> SearchAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken = default)
    {
        var message = await httpClient.GetAsync($"api/animals{request.ToQueryString()}", cancellationToken);
        message.EnsureSuccessStatusCode();

        var matches = await message.Content.ReadFromJsonAsync<IEnumerable<AnimalMatch>>(serializerOptions, cancellationToken);

        return matches ?? [];
    }

    public async Task<Result<AnimalResponse>> GetAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var message = await httpClient.GetAsync($"api/animals/{animalId}", cancellationToken);
        message.EnsureSuccessStatusCode();

        var response = await message.Content.ReadFromJsonAsync<AnimalResponse>(serializerOptions, cancellationToken);

        return response!;
    }

    public Task<Result<AnimalResponse>> AddAsync(AddAnimalRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AnimalResponse>> UpdateAsync(UpdateAnimalRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AnimalResponse>> AddImagesAsync(AddAnimalImagesCommand command,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
