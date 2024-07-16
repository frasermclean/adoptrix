using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Breeds;

public class UpdateBreedRequest
{
    public required string Name { get; init; }
    public Guid BreedId { get; init; }
    public Guid SpeciesId { get; init; }
    [FromClaim(ClaimTypes.UserId)] public Guid UserId { get; init; }
}
