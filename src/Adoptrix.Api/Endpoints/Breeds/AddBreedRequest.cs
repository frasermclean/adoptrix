using FastEndpoints;
using Microsoft.Identity.Web;

namespace Adoptrix.Api.Endpoints.Breeds;

public class AddBreedRequest
{
    public required string Name { get; init; }
    public Guid SpeciesId { get; init; }

    [FromClaim(ClaimConstants.Oid)] public Guid UserId { get; init; }
}
