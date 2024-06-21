using System.Net.Http.Json;
using Adoptrix.Domain.Errors;
using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Animals;
using FluentResults;
using MediatR;

namespace Adoptrix.Client.Handlers.Animals;

public class GetAnimalQueryHandler(HttpClient httpClient) : IRequestHandler<GetAnimalQuery, Result<AnimalResponse>>
{
    public async Task<Result<AnimalResponse>> Handle(GetAnimalQuery query, CancellationToken cancellationToken)
    {
        var message = await httpClient.GetAsync($"api/animals/{query.AnimalId}", cancellationToken);
        var response = await message.Content.ReadFromJsonAsync<AnimalResponse>(cancellationToken: cancellationToken);

        return response is null
            ? new AnimalNotFoundError(query.AnimalId)
            : response;
    }
}
