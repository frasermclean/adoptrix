using Adoptrix.Core.Requests;

namespace Adoptrix.Core.Extensions;

public static class AddBreedRequestExtensions
{
    public static Breed ToBreed(this AddBreedRequest request, Species species) => new()
    {
        Name = request.Name,
        Species = species,
        LastModifiedBy = request.UserId
    };
}
