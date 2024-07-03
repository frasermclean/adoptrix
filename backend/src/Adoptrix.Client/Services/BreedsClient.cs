using System.Net.Http.Json;
using System.Text.Json;
using Adoptrix.Client.Extensions;
using Adoptrix.Domain.Contracts.Requests;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Services;
using FluentResults;
using Microsoft.Extensions.Options;

namespace Adoptrix.Client.Services;

public class BreedsClient(HttpClient httpClient, JsonSerializerOptions serializerOptions) : IBreedsService
{
    public async Task<IEnumerable<BreedMatch>> SearchAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken = default)
    {
        var message = await httpClient.GetAsync($"api/breeds{request.ToQueryString()}", cancellationToken);
        message.EnsureSuccessStatusCode();

        var matches =
            await message.Content.ReadFromJsonAsync<IEnumerable<BreedMatch>>(serializerOptions, cancellationToken);

        return matches ?? [];
    }

    public Task<Result<BreedResponse>> GetAsync(Guid breedId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
