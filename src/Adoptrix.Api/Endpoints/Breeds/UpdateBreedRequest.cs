using FastEndpoints;
using Microsoft.Identity.Web;

namespace Adoptrix.Api.Endpoints.Breeds;

public class UpdateBreedRequest
{
    public required string Name { get; init; }
    public Guid BreedId { get; init; }
    public required string SpeciesName { get; init; }
    [FromClaim(ClaimConstants.Oid)] public Guid UserId { get; init; }
}
