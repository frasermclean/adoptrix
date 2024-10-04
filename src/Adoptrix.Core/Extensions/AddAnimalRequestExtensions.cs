using Adoptrix.Core.Requests;

namespace Adoptrix.Core.Extensions;

public static class AddAnimalRequestExtensions
{
    public static Animal ToAnimal(this AddAnimalRequest request, Breed breed) => new()
    {
        Name = request.Name,
        Description = request.Description,
        Breed = breed,
        Sex = request.Sex,
        DateOfBirth = request.DateOfBirth,
        Slug = Animal.CreateSlug(request.Name, request.DateOfBirth),
        LastModifiedBy = request.UserId
    };
}
