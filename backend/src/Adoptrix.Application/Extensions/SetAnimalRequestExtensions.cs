using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Domain.Models;

namespace Adoptrix.Application.Extensions;

public static class SetAnimalRequestExtensions
{
    public static Animal ToAnimal(this SetAnimalRequest request, Breed breed) => new()
    {
        Name = request.Name,
        Description = request.Description,
        Breed = breed,
        Sex = request.Sex,
        DateOfBirth = request.DateOfBirth,
        CreatedBy = request.UserId,
    };
}
