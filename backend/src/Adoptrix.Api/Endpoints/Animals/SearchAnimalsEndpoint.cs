using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Services.Repositories;

namespace Adoptrix.Api.Endpoints.Animals;

public static class SearchAnimalsEndpoint
{
    public static async Task<IEnumerable<AnimalResponse>> ExecuteAsync(string? name, Guid? speciesId, IAnimalsRepository repository,
        CancellationToken cancellationToken)
    {
        var results = await repository.SearchAsync(name, speciesId, cancellationToken);
        return results.Select(result => result.ToResponse());
    }
}
