using System.Net.Http.Json;
using Adoptrix.Client.Extensions;
using Adoptrix.Domain.Commands.Animals;
using Adoptrix.Domain.Contracts.Requests;
using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Services;
using FluentResults;

namespace Adoptrix.Client.Services;

public class AnimalsClient(HttpClient httpClient) : IAnimalsService
{
    public async Task<IEnumerable<AnimalMatch>> SearchAsync(SearchAnimalsRequest? request, CancellationToken cancellationToken = default)
    {
        var queryString = request?.ToQueryString() ?? string.Empty;
        var message = await httpClient.GetAsync($"api/species{queryString}", cancellationToken);
        message.EnsureSuccessStatusCode();

        var matches = await message.Content.ReadFromJsonAsync<IEnumerable<AnimalMatch>>(cancellationToken: cancellationToken);

        return matches ?? [];
    }

    public async Task<Result<AnimalResponse>> GetAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var message = await httpClient.GetAsync($"api/animals/{animalId}", cancellationToken);
        message.EnsureSuccessStatusCode();

        var response = await message.Content.ReadFromJsonAsync<AnimalResponse>(cancellationToken: cancellationToken);

        return response!;
    }

    public Task<Result<AnimalResponse>> AddAsync(AddAnimalCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AnimalResponse>> UpdateAsync(UpdateAnimalCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AnimalResponse>> AddImagesAsync(AddAnimalImagesCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
