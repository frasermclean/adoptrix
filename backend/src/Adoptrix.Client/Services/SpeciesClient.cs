using System.Net.Http.Json;
using System.Text.Json;
using Adoptrix.Client.Extensions;
using Adoptrix.Domain.Contracts.Requests;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Services;
using FluentResults;
using Microsoft.Extensions.Options;

namespace Adoptrix.Client.Services;

public class SpeciesClient(HttpClient httpClient, JsonSerializerOptions serializerOptions) : ISpeciesService
{
    public async Task<IEnumerable<SpeciesMatch>> SearchAsync(SearchSpeciesRequest request,
        CancellationToken cancellationToken = default)
    {
        var message = await httpClient.GetAsync($"api/species{request.ToQueryString()}", cancellationToken);
        message.EnsureSuccessStatusCode();

        var matches =
            await message.Content.ReadFromJsonAsync<IEnumerable<SpeciesMatch>>(serializerOptions, cancellationToken);

        return matches ?? [];
    }

    public Task<Result<SpeciesResponse>> GetAsync(Guid speciesId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
