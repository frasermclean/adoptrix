using System.Net.Http.Json;
using System.Text.Json;
using Adoptrix.Client.Extensions;
using Adoptrix.Core.Contracts.Requests.Breeds;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Core.Services;
using FluentResults;

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

    public Task<Result<BreedResponse>> AddAsync(AddBreedRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<BreedResponse>> UpdateAsync(UpdateBreedRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(Guid breedId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
