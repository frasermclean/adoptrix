using FastEndpoints;

namespace Adoptrix.Contracts.Requests;

public class AddBreedRequest
{
    public required string Name { get; init; }
    public required string SpeciesName { get; init; }

    [FromClaim(ClaimConstants.UserId)] public Guid UserId { get; init; }
}
