using Adoptrix.Application.Services;
using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Species;
using FastEndpoints;

namespace Adoptrix.Endpoints.Species;

public class SearchSpeciesEndpoint(ISpeciesRepository speciesRepository)
    : Endpoint<SearchSpeciesQuery, IEnumerable<SpeciesMatch>>
{
    public override void Configure()
    {
        Get("species");
        AllowAnonymous();
    }

    public override async Task<IEnumerable<SpeciesMatch>> ExecuteAsync(SearchSpeciesQuery query,
        CancellationToken cancellationToken)
    {
        return await speciesRepository.SearchAsync(query, cancellationToken);
    }
}
