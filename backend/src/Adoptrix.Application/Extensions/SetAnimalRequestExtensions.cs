using Adoptrix.Application.Contracts.Requests;
using Adoptrix.Domain.Models;

namespace Adoptrix.Application.Extensions;

public static class SetAnimalRequestExtensions
{
    public static Animal ToAnimal(this SetAnimalRequest request, Breed breed, Guid createdBy) => new()
    {
        Name = request.Name,
        Description = request.Description,
        Breed = breed,
        Sex = request.Sex,
        DateOfBirth = request.DateOfBirth,
        CreatedBy = createdBy,
    };
}
