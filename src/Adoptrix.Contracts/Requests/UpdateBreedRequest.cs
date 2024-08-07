using FastEndpoints;

namespace Adoptrix.Contracts.Requests;

public class UpdateBreedRequest
{
    public required string Name { get; init; }
    public int BreedId { get; init; }
    public required string SpeciesName { get; init; }
    [FromClaim(ClaimConstants.UserId)] public Guid UserId { get; init; }
}
