using Adoptrix.Contracts.Responses;
using Adoptrix.Persistence.Responses;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Api.Mapping;

[Mapper]
public static partial class AnimalMatchMapper
{
    public static partial AnimalMatch ToMatch(this SearchAnimalsItem item);
}
