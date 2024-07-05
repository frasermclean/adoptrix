using Adoptrix.Domain;
using Adoptrix.Domain.Contracts.Responses;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Application.Mapping;

[Mapper]
public static partial class AnimalImageMapper
{
    public static partial AnimalImageResponse ToResponse(this AnimalImage animalImage);
}
