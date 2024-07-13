using Adoptrix.Core;
using Adoptrix.Core.Contracts.Responses;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Mapping;

[Mapper]
public static partial class AnimalImageMapper
{
    public static partial AnimalImageResponse ToResponse(this AnimalImage animalImage);
}

