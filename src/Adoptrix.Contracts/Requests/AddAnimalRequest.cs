using FastEndpoints;

namespace Adoptrix.Contracts.Requests;

public class AddAnimalRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public int BreedId { get; init; }
    public required string Sex { get; init; }
    public DateOnly DateOfBirth { get; init; }
    [FromClaim(ClaimConstants.UserId)] public Guid UserId { get; init; }
}
