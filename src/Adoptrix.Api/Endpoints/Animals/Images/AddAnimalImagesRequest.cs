using Microsoft.Identity.Web;

namespace Adoptrix.Api.Endpoints.Animals.Images;

public class AddAnimalImagesRequest
{
    public int AnimalId { get; init; }
    [FromClaim(ClaimConstants.Oid)] public Guid UserId { get; init; }
}
