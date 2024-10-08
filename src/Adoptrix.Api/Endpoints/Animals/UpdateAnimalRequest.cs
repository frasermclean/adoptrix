using Adoptrix.Core;
using Microsoft.Identity.Web;

namespace Adoptrix.Api.Endpoints.Animals;

public class UpdateAnimalRequest
{
    public Guid AnimalId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required string BreedName { get; init; }
    public required Sex Sex { get; init; }
    public DateOnly DateOfBirth { get; init; }
    [FromClaim(ClaimConstants.Oid)] public Guid UserId { get; init; }
}
