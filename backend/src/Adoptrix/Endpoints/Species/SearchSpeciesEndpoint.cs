using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Species;
using FastEndpoints;
using MediatR;

namespace Adoptrix.Endpoints.Species;

public class SearchSpeciesEndpoint(ISender sender) : Endpoint<SearchSpeciesQuery, IEnumerable<SpeciesMatch>>
{
    public override void Configure()
    {
        Get("species");
        AllowAnonymous();
    }

    public override async Task<IEnumerable<SpeciesMatch>> ExecuteAsync(SearchSpeciesQuery query,
        CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }
}
