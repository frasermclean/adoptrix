using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Domain.Models;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Api.Mapping;

[Mapper]
public static partial class SpeciesMapper
{
    public static partial SpeciesResponse ToResponse(this Species species);
}