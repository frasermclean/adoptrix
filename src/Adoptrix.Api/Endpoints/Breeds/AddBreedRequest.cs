using FastEndpoints;
using Microsoft.Identity.Web;

namespace Adoptrix.Api.Endpoints.Breeds;

public class AddBreedRequest
{
    public required string Name { get; init; }
    public required string SpeciesName { get; init; }

    [FromClaim(ClaimConstants.Oid)] public Guid UserId { get; init; }
}
