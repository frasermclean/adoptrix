using Adoptrix.Core;
using FastEndpoints;
using Microsoft.Identity.Web;

namespace Adoptrix.Api.Endpoints.Animals;

public class AddAnimalRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public Guid BreedId { get; init; }
    public Sex Sex { get; init; }
    public DateOnly DateOfBirth { get; init; }
    [FromClaim(ClaimConstants.Oid)] public Guid UserId { get; init; }
}
