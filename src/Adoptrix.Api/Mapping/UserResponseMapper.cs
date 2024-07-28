using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Riok.Mapperly.Abstractions;

namespace Adoptrix.Api.Mapping;

[Mapper]
public static partial class UserResponseMapper
{
    public static partial UserResponse ToResponse(this User user);
}
