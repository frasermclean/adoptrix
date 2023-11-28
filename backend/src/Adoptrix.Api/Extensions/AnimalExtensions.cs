using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Application.Services;
using Adoptrix.Domain;

namespace Adoptrix.Api.Extensions;

public static class AnimalExtensions
{
    public static AnimalResponse ToResponse(this Animal animal, ISqidConverter sqidConverter) => new()
    {
        Id = sqidConverter.ConvertToSqid(animal.Id),
        Name = animal.Name,
        Description = animal.Description,
        Species = animal.Species,
        DateOfBirth = animal.DateOfBirth
    };
}