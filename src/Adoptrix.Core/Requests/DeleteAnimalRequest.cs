using FastEndpoints;

namespace Adoptrix.Core.Requests;

public class DeleteAnimalRequest
{
    public Guid AnimalId { get; init; }
    [FromClaim(RequestClaims.UserId)] public Guid UserId { get; init; }
}
