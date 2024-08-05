using Adoptrix.Api.Mapping;
using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Adoptrix.Persistence.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Adoptrix.Api.Endpoints.Animals;

[HttpGet("animals"), AllowAnonymous]
public class SearchAnimalsEndpoint(IAnimalsRepository animalsRepository)
    : Endpoint<SearchAnimalsRequest, IEnumerable<AnimalMatch>>
{
    public override async Task<IEnumerable<AnimalMatch>> ExecuteAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken)
    {
        Sex? sex = Enum.TryParse<Sex>(request.Sex, true, out var value)
            ? value
            : null;

        var items = await animalsRepository.SearchAsync(request.Name, request.BreedId, request.SpeciesName, sex,
            request.Limit, cancellationToken);

        return items.Select(item => item.ToMatch());
    }
}
