using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Models;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Application.Mapping;

[Mapper]
public static partial class AnimalImageMapper
{
    public static partial AnimalImageResponse ToResponse(this AnimalImage animalImage);
}
