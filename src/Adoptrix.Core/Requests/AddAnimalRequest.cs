using FastEndpoints;

namespace Adoptrix.Core.Requests;

public class AddAnimalRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public int BreedId { get; init; }
    public required Sex Sex { get; init; }
    public DateOnly DateOfBirth { get; init; }
    [FromClaim(RequestClaims.UserId)] public Guid UserId { get; init; }
}
