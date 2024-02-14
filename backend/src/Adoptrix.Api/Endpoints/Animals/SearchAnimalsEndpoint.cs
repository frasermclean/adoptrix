using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Mapping;
using Adoptrix.Application.Services.Repositories;

namespace Adoptrix.Api.Endpoints.Animals;

public static class SearchAnimalsEndpoint
{
    public static async Task<IEnumerable<AnimalResponse>> ExecuteAsync(string? name, string? species, IAnimalsRepository repository,
        CancellationToken cancellationToken)
    {
        var results = await repository.SearchAnimalsAsync(name, species, cancellationToken);
        return results.Select(result => result.ToResponse());
    }
}