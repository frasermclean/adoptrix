using Adoptrix.Domain.Contracts.Requests;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Services;
using FastEndpoints;
using MediatR;

namespace Adoptrix.Endpoints.Species;

public class SearchSpeciesEndpoint(ISpeciesService speciesService) : Endpoint<SearchSpeciesRequest, IEnumerable<SpeciesMatch>>
{
    public override void Configure()
    {
        Get("species");
        AllowAnonymous();
    }

    public override async Task<IEnumerable<SpeciesMatch>> ExecuteAsync(SearchSpeciesRequest request,
        CancellationToken cancellationToken)
    {
        return await speciesService.SearchAsync(request, cancellationToken);
    }
}
