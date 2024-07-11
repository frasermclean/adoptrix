using Adoptrix.Core.Contracts;
using FastEndpoints;

namespace Adoptrix.Endpoints.Animals.Images;

public class AddAnimalImagesRequest
{
    public Guid AnimalId { get; init; }
    [FromClaim(ClaimTypes.UserId)] public Guid UserId { get; init; }
}
