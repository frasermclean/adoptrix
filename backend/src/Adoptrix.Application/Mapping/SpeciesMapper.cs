using Adoptrix.Domain;
using Adoptrix.Domain.Contracts.Responses;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Application.Mapping;

[Mapper]
public static partial class SpeciesMapper
{
    public static partial SpeciesResponse ToResponse(this Species species);
}
