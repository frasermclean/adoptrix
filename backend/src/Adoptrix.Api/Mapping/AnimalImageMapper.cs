using Adoptrix.Application.Models;
using Adoptrix.Domain;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Api.Mapping;

[Mapper]
public static partial class AnimalImageMapper
{
    public static partial AnimalImageResponse ToResponse(this AnimalImage animalImage);
}
