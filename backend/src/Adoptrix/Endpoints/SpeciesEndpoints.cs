using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Species;
using MediatR;

namespace Adoptrix.Endpoints;

public static class SpeciesEndpoints
{
    public static async Task<IEnumerable<SpeciesMatch>> SearchSpecies([AsParameters] SearchSpeciesQuery query,
        ISender sender, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }
}
