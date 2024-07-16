using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Animals.Images;

public class AddAnimalImagesRequest
{
    public Guid AnimalId { get; init; }
    [FromClaim(ClaimTypes.UserId)] public Guid UserId { get; init; }
}
