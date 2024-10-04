using FastEndpoints;

namespace Adoptrix.Core.Requests;

public class UpdateBreedRequest
{
    public required string Name { get; init; }
    public int BreedId { get; init; }
    public required string SpeciesName { get; init; }
    [FromClaim(RequestClaims.UserId)] public Guid UserId { get; init; }
}
