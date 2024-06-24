using Adoptrix.Application.Services;
using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Breeds;
using FastEndpoints;
using MediatR;

namespace Adoptrix.Endpoints.Breeds;

public class SearchBreedsEndpoint(ISender sender) : Endpoint<SearchBreedsQuery, IEnumerable<BreedMatch>>
{
    public override void Configure()
    {
        Get("breeds");
        AllowAnonymous();
    }

    public override async Task<IEnumerable<BreedMatch>> ExecuteAsync(SearchBreedsQuery query,
        CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }
}
