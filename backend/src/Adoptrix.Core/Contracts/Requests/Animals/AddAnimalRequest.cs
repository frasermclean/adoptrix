using FastEndpoints;

namespace Adoptrix.Core.Contracts.Requests.Animals;

public class AddAnimalRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public Guid BreedId { get; init; }
    public Sex Sex { get; init; }
    public DateOnly DateOfBirth { get; init; }
    [FromClaim(ClaimTypes.UserId)] public Guid UserId { get; init; }
}
