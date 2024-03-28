using Adoptrix.Application.Models;
using Adoptrix.Application.Services;

namespace Adoptrix.Api.Endpoints.Animals;

public static class SearchAnimalsEndpoint
{
    public static async Task<IEnumerable<SearchAnimalsResult>> ExecuteAsync(string? name, Guid? speciesId,
        IAnimalsRepository repository, CancellationToken cancellationToken)
    {
        return await repository.SearchAsync(name, speciesId, cancellationToken);
    }
}
