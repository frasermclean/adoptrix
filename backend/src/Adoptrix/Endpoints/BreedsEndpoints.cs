using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Breeds;
using MediatR;

namespace Adoptrix.Endpoints;

public static class BreedsEndpoints
{
    public static async Task<IEnumerable<BreedMatch>> SearchBreeds([AsParameters] SearchBreedsQuery query,
        ISender sender, CancellationToken cancellationToken)
    {
        return await sender.Send(query, cancellationToken);
    }
}
